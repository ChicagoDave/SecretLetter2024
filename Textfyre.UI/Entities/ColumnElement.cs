﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Textfyre.UI.Entities
{
    public class ColumnElement
    {
        public OpCode OpCode;
        public string Data;

        public ColumnElement(OpCode opCode)
        {
            OpCode = opCode;
            Data = String.Empty;
        }

        public ColumnElement(OpCode opCode, string data)
        {
            OpCode = opCode;
            Data = data;
        }
    }
}
