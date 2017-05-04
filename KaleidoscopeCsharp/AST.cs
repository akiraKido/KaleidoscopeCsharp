using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaleidoscopeCsharp
{
    /// <summary>
    /// Expression のベースクラス
    /// </summary>
    internal abstract class ExpressionAST { }

    /// <summary>
    /// 数字用（1.0など）のクラス
    /// </summary>
    internal class NumberExpressionAST : ExpressionAST
    {
        private readonly double value;

        internal NumberExpressionAST(double value) { this.value = value; }
    }

    /// <summary>
    /// 変数用
    /// </summary>
    internal class VariableExpressionAST : ExpressionAST
    {
        private readonly string name;

        internal VariableExpressionAST(string name) { this.name = name; }
    }

    /// <summary>
    /// 数式用
    /// </summary>
    internal class BinaryExpressionAST : ExpressionAST
    {
        private readonly char operation;
        private readonly ExpressionAST leftHand;
        private readonly ExpressionAST rightHand;

        internal BinaryExpressionAST(
            char operation,
            ExpressionAST leftHand,
            ExpressionAST rightHand)
        {
            this.operation = operation;
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }
    }

    /// <summary>
    /// 関数呼び出し用
    /// </summary>
    internal class CallExpressionAST : ExpressionAST
    {
        private readonly string callee;
        private readonly List<ExpressionAST> args;

        internal CallExpressionAST(string callee, List<ExpressionAST> args)
        {
            this.callee = callee;
            this.args = args;
        }
    }

    /// <summary>
    /// 関数の宣言用
    /// </summary>
    class PrototypeAST
    {
        private readonly string name;
        private readonly List<string> args;

        internal PrototypeAST(string name, List<string> args)
        {
            this.name = name;
            this.args = args;
        }
    }

    /// <summary>
    /// 関数本体
    /// </summary>
    class FunctionAST
    {
        private readonly PrototypeAST prototype;
        private readonly ExpressionAST body;

        internal FunctionAST(PrototypeAST prototype, ExpressionAST body)
        {
            this.prototype = prototype;
            this.body = body;
        }
    }

}
