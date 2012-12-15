using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Machine.Specifications;

namespace clisp.test.lexer
{
    [Subject("Tokenizer")]
    public class when_empty_list
    {
        private Establish context = () =>
                                        {
                                            _exp = "()";
                                            _lexer=new Lexer();
                                        };
        private Because of = () => _ret = _lexer.Tokenizer(_exp);
        private It should_return_two_tokens = () => _ret.Count().ShouldEqual(2);
        private It first_should_be_open_parenthesis = () => _ret.First().ShouldEqual("(");
        private It last_should_be_close_parenthesis = () => _ret.Last().ShouldEqual(")");
        private static IEnumerable<string> _ret;
        private static Lexer _lexer;
        private static string _exp;
    }

    [Subject("Tokenizer")]
    public class when_list_with_forms
    {
        private Establish context = () =>
        {
            _exp = "(1 2 3)";
            _lexer = new Lexer();
        };
        private Because of = () => _ret = _lexer.Tokenizer(_exp);
        private It should_return_two_plus_number_of_forms = () => _ret.Count().ShouldEqual(5);
        private It first_should_be_open_parenthesis = () => _ret.First().ShouldEqual("(");
        private It last_should_be_close_parenthesis = () => _ret.Last().ShouldEqual(")");
        private static IEnumerable<string> _ret;
        private static Lexer _lexer;
        private static string _exp;
    }

    [Subject("Tokenizer")]
    public class when_complex_list
    {
        private Establish context = () =>
        {
            _exp = "(defun doble (x) (* x 25))";
            _lexer = new Lexer();
        };
        private Because of = () => _ret = _lexer.Tokenizer(_exp);
        private It should_return_two_plus_number_of_forms = () => _ret.Count().ShouldEqual(12);
        private It second_should_be_defun = () => _ret.Skip(1).First().ShouldEqual("defun");
        private It eighth_should_be_star = () => _ret.Skip(7).First().ShouldEqual("*");
        private static IEnumerable<string> _ret;
        private static Lexer _lexer;
        private static string _exp;
    }

    public class Lexer
    {
        public IEnumerable<string> Tokenizer(string exp)
        {
            var parts = Regex.Matches(exp, @"[\(\)]|[^\(\)\s]+");

            return from Match part in parts select part.Value;
        }
    }

}
