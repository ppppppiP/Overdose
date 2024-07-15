using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
namespace BlackBoardSystem
{

    [Serializable]
    public readonly struct BlackboardKey: IEquatable<BlackboardKey>
    {
        readonly string name;
        readonly int hashedKey;

        public BlackboardKey(string name)
        {
            this.name = name;
            hashedKey = name.ComputeFNV1aHash();
        }

        public bool Equals(BlackboardKey other)
        {
            return hashedKey == other.hashedKey;

        }
        public override bool Equals(object obj)
        {
            return obj is BlackboardKey other && Equals(other); 
        }
        public override int GetHashCode()
        {
            return hashedKey;
        }
        public override string ToString()
        {
            return name;
        }
        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey; 
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => !(lhs == rhs);
    }

    [Serializable]
    public class BlackboardEntry<T>
    {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }

        public BlackboardEntry(BlackboardKey key, T value)
        {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }
        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key; 
        public override int GetHashCode() => Key.GetHashCode();


    }

    [Serializable]
    public class Blackboard
    {
        Dictionary<string, BlackboardKey> keyRegistry = new(); 
        Dictionary<BlackboardKey, object> entries = new();

        public List<Action> PassedActions { get; } = new();
        public void AddAction(Action action)
        {
            Preconditions.CheckNotNull(action);
            PassedActions.Add(action);
        }

        public void ClearActions() => PassedActions.Clear();

        public void Debug()
        {
            foreach(var entrie in entries)
            {
                var entriType = entrie.Value.GetType();

                if(entriType.IsGenericType && entriType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>))
                {
                    var valueProperty = entriType.GetProperty("Value");
                    if (valueProperty == null) continue;
                    var value = valueProperty.GetValue(entrie.Value);
                    UnityEngine.Debug.Log($"Key: {entrie.Key} Value: {value}");
                }
            }
        }

        public bool TryGetValue<T>(BlackboardKey key, out T value)
        {
            if (entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry)
            {
                value = castedEntry.Value;
                return true;
                
            }
            value = default;
            return false;
        }

        public void SetValue<T>(BlackboardKey key, T value)
        {
            entries[key] = new BlackboardEntry<T>(key, value);
        }

        public BlackboardKey GetOrRegisterKey(string keyName)
        {
            Preconditions.CheckNotNull(keyName);
            if(!keyRegistry.TryGetValue(keyName, out BlackboardKey value))
            {
                value = new BlackboardKey(keyName);
                keyRegistry[keyName] = value;
            }
            return value;

        }

        public bool ContainsKey(BlackboardKey key) => entries.ContainsKey(key);
        public void Remove(BlackboardKey key) => entries.Remove(key);

    }
}

public static class StringExtention
{
    public static int ComputeFNV1aHash(this string str)
    {
        uint hash = 2166136261;
        foreach(char c in str)
        {
            hash = (hash ^ c) * 16777619;
        }
        return unchecked((int)hash);
    }
    
}
public class Preconditions
{
    Preconditions() { }

    public static T CheckNotNull<T>(T reference)
    {
        return CheckNotNull(reference, null);
    }

    public static T CheckNotNull<T>(T reference, string message)
    {
        // Can find OrNull Extension Method (and others) here: https://github.com/adammyhre/Unity-Utils
        if (reference is UnityEngine.Object obj && obj.OrNull() == null)
        {
            throw new ArgumentNullException(message);
        }
        if (reference is null)
        {
            throw new ArgumentNullException(message);
        }
        return reference;
    }

    public static void CheckState(bool expression)
    {
        CheckState(expression, null);
    }

    public static void CheckState(bool expression, string messageTemplate, params object[] messageArgs)
    {
        CheckState(expression, string.Format(messageTemplate, messageArgs));
    }

    public static void CheckState(bool expression, string message)
    {
        if (expression)
        {
            return;
        }

        throw message == null ? new InvalidOperationException() : new InvalidOperationException(message);
    }


}