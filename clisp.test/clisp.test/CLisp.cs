using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Machine.Specifications;
using clisp.test.lexer;


namespace clisp.test.clips
{
    [Subject("EvaluateExpression")]
    public class when_atom_is_data
    {
        private Establish context = () =>
        {
            _exp = "5 \"abc\"";
            _clisp = new CLispMachine();
        };
        private Because of = () => { _ret = _clisp.Evaluate(null, _exp); };
        private It should_return_itself = () => _ret.ShouldEqual("5\n\"abc\"");

        private static object _ret;
        private static string _exp;
        private static CLispMachine _clisp;
    }

    [Subject("EvaluateExpression")]
    public class when_atom_is_an_undefined_variable
    {
        private Establish context = () =>
        {
            _exp = "abc";
            _clisp = new CLispMachine();
        };
        private Because of = () =>
        {
            _exception = Catch.Exception(() => _clisp.Evaluate(new Dictionary<string, string>(), _exp));
        };

        private It should_return_a_exception = () => _exception.ShouldBeOfType<Exception>();

        private static string _exp;
        private static CLispMachine _clisp;
        private static Exception _exception;
    }

    [Subject("EvaluateExpression")]
    public class when_atom_is_a_defined_variable
    {
        private Establish context = () =>
        {
            _exp = "abc";
            _clisp = new CLispMachine();
        };
        private Because of = () =>
        {
            _ret = _clisp.Evaluate(new Dictionary<string, string> { { "abc", "5" } }, _exp);
        };

        private It should_return_value = () => _ret.ShouldEqual("5");

        private static string _exp;
        private static CLispMachine _clisp;
        private static string _ret;
    }

    [Subject("EvaluateExpression")]
    public class when_atom_is_nil
    {
        private Establish context = () =>
        {
            _exp = "NIL";
            _clisp = new CLispMachine();
        };
        private Because of = () =>
        {
            _ret = _clisp.Evaluate(new Dictionary<string, string>(), _exp);
        };

        private It should_return_value = () => _ret.ShouldEqual("NIL");

        private static string _exp;
        private static CLispMachine _clisp;
        private static string _ret;
    }

    [Subject("EvaluateExpression")]
    public class when_atom_is_T
    {
        private Establish context = () =>
        {
            _exp = "T";
            _clisp = new CLispMachine();
        };
        private Because of = () =>
        {
            _ret = _clisp.Evaluate(new Dictionary<string, string>(), _exp);
        };

        private It should_return_value = () => _ret.ShouldEqual("T");

        private static string _exp;
        private static CLispMachine _clisp;
        private static string _ret;
    }


    internal class EnvironmentMock
    {
    }

    [Subject("EvaluateExpression")]
    public class when_empty_list
    {
        private Establish context = () =>
        {
            _exp = "()";
            _clisp = new CLispMachine();
        };
        private Because of = () =>
        {
            _ret = _clisp.Evaluate(new Dictionary<string, string>(), _exp);
        };

        private It should_return_nil = () => _ret.ShouldEqual("NIL");

        private static object _ret;
        private static string _exp;
        private static CLispMachine _clisp;
    }


    public class CLispMachine
    {
        public string Evaluate(Dictionary<string, string> memory, string exp)
        {
            var result = "";
            var tokens = new Lexer().Tokenizer(exp);
            if (tokens.Any() && tokens.First() == "(")
                return "NIL";

            foreach (var token in tokens)
            {
                if (Regex.IsMatch(token, @"[\d]+") || Regex.IsMatch(token, "\\\".+\\\""))
                {
                    result += token + "\n";
                }
                else
                {
                    switch (token)
                    {
                        case "NIL":
                            return "NIL";
                        case "T":
                            return "T";
                        default:
                            break;
                    }
                    if (memory.ContainsKey(token))
                        return memory[token];
                    throw new Exception();
                }
            }

            return result.TrimEnd('\n');
        }
    }
}
