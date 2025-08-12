using System;
using System.Collections.Generic;


namespace Gamify.SnakeGame.MessagePostSystem
{
    /// <summary>
    /// Singleton MessagePost system for broadcasting messages with no parameters.
    /// Supports normal and one-off listeners.
    /// </summary>
    public class MessagePost
    {
        private static MessagePost _instance;
        public static MessagePost Instance => _instance ??= new MessagePost();

        private readonly Dictionary<int, HashSet<Action>> _messageActions;
        private readonly Dictionary<int, List<Action>> _oneOffActions;

        private MessagePost()
        {
            _messageActions = new Dictionary<int, HashSet<Action>>();
            _oneOffActions = new Dictionary<int, List<Action>>();
        }

        public void Listen(MessageIDs msgID, Action handleAction, bool listenOnce = false)
        {
            Listen((int)msgID, handleAction, listenOnce);
        }

        public void Listen(int msgID, Action handleAction, bool listenOnce = false)
        {
            if (!_messageActions.TryGetValue(msgID, out var listeners))
            {
                listeners = new HashSet<Action>();
                _messageActions[msgID] = listeners;
            }

            listeners.Add(handleAction);

            if (listenOnce)
            {
                if (!_oneOffActions.TryGetValue(msgID, out var oneOffList))
                {
                    oneOffList = new List<Action>();
                    _oneOffActions[msgID] = oneOffList;
                }
                if (!oneOffList.Contains(handleAction))
                {
                    oneOffList.Add(handleAction);
                }
            }
        }

        public void Unlisten(MessageIDs msgID, Action handleAction)
        {
            Unlisten((int)msgID, handleAction);
        }

        public void Unlisten(int msgID, Action handleAction)
        {
            if (_messageActions.TryGetValue(msgID, out var listeners))
            {
                listeners.Remove(handleAction);
                if (listeners.Count == 0)
                    _messageActions.Remove(msgID);
            }

            if (_oneOffActions.TryGetValue(msgID, out var oneOffList))
            {
                oneOffList.Remove(handleAction);
                if (oneOffList.Count == 0)
                    _oneOffActions.Remove(msgID);
            }
        }

        public void ClearListen(int msgID)
        {
            _messageActions.Remove(msgID);
            _oneOffActions.Remove(msgID);
        }

        public void ClearAllListens()
        {
            _messageActions.Clear();
            _oneOffActions.Clear();
        }

        public void Broadcast(MessageIDs msgID)
        {
            Broadcast((int)msgID);
        }

        public void Broadcast(int msgID)
        {
            if (_messageActions.TryGetValue(msgID, out var listeners))
            {
                // To avoid modification during iteration
                var invocationList = new List<Action>(listeners);
                foreach (var listener in invocationList)
                {
                    listener.Invoke();
                }

                // Remove one-off listeners after invocation
                if (_oneOffActions.TryGetValue(msgID, out var oneOffList))
                {
                    foreach (var oneOffListener in oneOffList)
                    {
                        listeners.Remove(oneOffListener);
                    }
                    _oneOffActions.Remove(msgID);
                }

                if (listeners.Count == 0)
                    _messageActions.Remove(msgID);
            }
        }

        public void PrintEverything()
        {
            foreach (var item in _messageActions)
            {
                UnityEngine.Debug.Log($"Message: {item.Key} has {item.Value.Count} listeners");
            }
        }
    }
}
