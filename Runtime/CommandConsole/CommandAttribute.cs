using System;
using System.Collections;
using UnityEngine;

namespace PunIntended.Tools
{
    /// <summary>
    /// makes method available to call from the command console
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Alias { get; private set; }
        public CommandExectionType ExecutionType { get; private set; }

        public CommandAttribute()
        {
            Alias = string.Empty;
        }

        public CommandAttribute(string alias)
        {
            Alias = alias;
        }

        public CommandAttribute(CommandExectionType executionType)
        {
            Alias = string.Empty;
            ExecutionType = executionType;
        }

        public CommandAttribute(string alias, CommandExectionType executionType)
        {
            Alias = alias;
            ExecutionType = executionType;
        }
    }

    /// <summary>
    /// how the command should be called
    /// </summary>
    public enum CommandExectionType
    {
        FindAllObjectsOfType =  0,
        FindFirstObjectOfType = 1
    }
}