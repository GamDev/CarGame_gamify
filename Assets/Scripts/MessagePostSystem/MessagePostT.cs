using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.SnakeGame.MessagePostSystem
{

    /// <summary>
    /// Singleton message bus supporting messages with one parameter of type T.
    /// Allows listening, broadcasting, and one-time listeners.
    /// </summary>
    public class MessagePost<T>
    {
        private static MessagePost<T> _instance;
        public static MessagePost<T> Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MessagePost<T>();
                return _instance;
            }
        }

        private MessagePost()
        {
            _messageActions = new Dictionary<int, Action<T>>();
            _oneOffActions = new Dictionary<int, List<Delegate>>();
        }

        private Dictionary<int, Action<T>> _messageActions;
        private Dictionary<int, List<Delegate>> _oneOffActions;

        /// <summary>
        /// Listen for a message with the given ID.
        /// </summary>
        /// <param name="msgID">Message ID</param>
        /// <param name="handleAction">Action to invoke on broadcast</param>
        /// <param name="listenOnce">If true, listener will be removed after first invocation</param>
        public void Listen(MessageIDs msgID, Action<T> handleAction, bool listenOnce = false)
        {
            Listen((int)msgID, handleAction, listenOnce);
        }

        public void Listen(int msgID, Action<T> handleAction, bool listenOnce = false)
        {
            if (_messageActions.TryGetValue(msgID, out var existing))
            {
                if (!existing.GetInvocationList().Contains(handleAction))
                {
                    _messageActions[msgID] += handleAction;
                }
            }
            else
            {
                _messageActions[msgID] = handleAction;
            }

            if (listenOnce)
            {
                if (!_oneOffActions.TryGetValue(msgID, out var list))
                {
                    list = new List<Delegate>();
                    _oneOffActions[msgID] = list;
                }

                if (!list.Contains(handleAction))
                {
                    list.Add(handleAction);
                }
            }
        }

        /// <summary>
        /// Unlisten (remove) a previously registered listener.
        /// </summary>
        public void Unlisten(MessageIDs msgID, Action<T> handleAction)
        {
            Unlisten((int)msgID, handleAction);
        }

        public void Unlisten(int msgID, Action<T> handleAction)
        {
            if (_messageActions.TryGetValue(msgID, out var existing))
            {
                existing -= handleAction;
                if (existing == null)
                    _messageActions.Remove(msgID);
                else
                    _messageActions[msgID] = existing;
            }

            if (_oneOffActions.TryGetValue(msgID, out var list))
            {
                list.Remove(handleAction);
                if (list.Count == 0)
                    _oneOffActions.Remove(msgID);
            }
        }

        /// <summary>
        /// Remove all listeners for a specific message ID.
        /// </summary>
        public void ClearListen(int msgID)
        {
            _messageActions.Remove(msgID);
            _oneOffActions.Remove(msgID);
        }

        /// <summary>
        /// Remove all listeners for all message IDs.
        /// </summary>
        public void ClearAllListens()
        {
            _messageActions.Clear();
            _oneOffActions.Clear();
        }

        /// <summary>
        /// Broadcast a message with parameter to all listeners.
        /// One-time listeners will be removed after invocation.
        /// </summary>
        public void Broadcast(MessageIDs msgID, T param)
        {
            Broadcast((int)msgID, param);
        }

        public void Broadcast(int msgID, T param)
        {
            if (_messageActions.TryGetValue(msgID, out var action))
            {
                action?.Invoke(param);

                if (_oneOffActions.TryGetValue(msgID, out var oneOffs))
                {
                    foreach (var oneOff in oneOffs)
                    {
                        action -= (Action<T>)oneOff;
                    }

                    // Update or remove entry based on remaining listeners
                    if (action == null)
                        _messageActions.Remove(msgID);
                    else
                        _messageActions[msgID] = action;

                    _oneOffActions.Remove(msgID);
                }
            }
        }
    }
}
