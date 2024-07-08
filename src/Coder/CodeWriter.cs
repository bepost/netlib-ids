using System;
using System.Collections.Generic;
using System.Text;

namespace Fujiberg.Coder;

public sealed class CodeWriter
{
    private readonly string _defaultIndent;
    private readonly StringBuilder _fileBuilder = new();
    private readonly Stack<int> _indentStack = [];
    private readonly StringBuilder _lineBuilder = new();
    private string _indent;

    public CodeWriter(string indent = "", string defaultIndent = "    ")
    {
        _indent = indent;
        _defaultIndent = defaultIndent;
    }

    public IDisposable Block(string? startSegment = "\r{\n", string? endSegment = "\r}\n", string? indentation = null)
    {
        indentation ??= _defaultIndent;

        if (startSegment is not null)
            Write(startSegment);

        return Indent(
            indentation,
            () => {
                if (endSegment is not null)
                    Write(endSegment);
            }
        );
    }

    public IDisposable Indent(string? indentation = null, Action? afterwards = null)
    {
        indentation ??= _defaultIndent;
        PushIndent(indentation);
        return new Disposable(
            () => {
                PopIndent();
                afterwards?.Invoke();
            }
        );
    }

    public override string ToString()
    {
        return string.Concat(
            _fileBuilder.ToString(),
            _lineBuilder.ToString()
                .TrimEnd()
        );
    }

    public CodeWriter Write(string code)
    {
        if (string.IsNullOrEmpty(code))
            return this;

        Append(code);
        return this;
    }

    public CodeWriter WriteIf(bool condition, string code)
    {
        if (!condition)
            return this;

        if (string.IsNullOrEmpty(code))
            return this;

        Append(code);
        return this;
    }

    public CodeWriter WriteLine(string code = "")
    {
        Write(code);
        AppendLine();
        return this;
    }

    public CodeWriter WriteLineIf(bool condition, string code = "")
    {
        if (!condition)
            return this;

        Write(code);
        AppendLine();
        return this;
    }

    public CodeWriter WriteLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
            WriteLine(line);
        return this;
    }

    private void Append(string code)
    {
        for (var index = 0; index < code.Length; index++)
        {
            var c = code[index];
            switch (c)
            {
                case '\n':
                    AppendLine();
                    break;
                case '\r':
                    if (index + 1 < code.Length && code[index + 1] == '\n')
                        break;
                    if (_lineBuilder.Length > 0)
                        AppendLine();
                    break;
                case '\t' when _lineBuilder.Length == 0:
                    _lineBuilder.Append(_defaultIndent);
                    break;
                default:
                    if (_lineBuilder.Length == 0)
                        _lineBuilder.Append(_indent);
                    _lineBuilder.Append(c);
                    break;
            }
        }
    }

    private void AppendLine()
    {
        _fileBuilder.AppendLine(
            _lineBuilder.ToString()
                .TrimEnd()
        );
        _lineBuilder.Clear();
    }

    private void PopIndent()
    {
        var length = _indentStack.Pop();
        _indent = _indent[..^length];
    }

    private void PushIndent(string? indentation = null)
    {
        indentation ??= _defaultIndent;
        _indentStack.Push(indentation.Length);
        _indent += indentation;
    }

    private sealed class Disposable : IDisposable
    {
        private readonly Action _action;

        public Disposable(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            _action();
        }
    }
}
