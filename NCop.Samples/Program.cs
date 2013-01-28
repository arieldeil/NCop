﻿using System;
using System.Linq;
using System.Reflection;
using NCop.Aspects.Aspects;
using NCop.Aspects.Framework;
using NCop.Composite.Framework;
using NCop.Core.Extensions;
using NCop.Mixins.Framework;
using NCop.Aspects.Advices;
using NCop.Aspects.Engine;
using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using NCop.IoC;
using System.Collections.Generic;
using NCop.Aspects.Weaving;
using NCop.Weaving;
using System.Threading;

namespace NCop.Samples
{
    internal static class Aspects
    {
        public static CSharpDeveloperMixin mixin = null;
        public static TraceAspect traceAspect = null;
        public static TraceAspect2 traceAspect2 = null;
        public static TraceAspect3 traceAspect3 = null;

        static Aspects() {
            mixin = new CSharpDeveloperMixin();
            traceAspect = new TraceAspect();
            traceAspect2 = new TraceAspect2();
            traceAspect3 = new TraceAspect3();
        }
    }

    public sealed class MethodDecoratorFunctionBinding : IFunctionBinding<CSharpDeveloperMixin, string, string>
    {
        public static MethodDecoratorFunctionBinding singleton = null;

        static MethodDecoratorFunctionBinding() {
            singleton = new MethodDecoratorFunctionBinding();
        }

        private MethodDecoratorFunctionBinding() {
        }

        public string Invoke(ref CSharpDeveloperMixin instance, IFunctionArgs<string, string> args) {
            return instance.Code(args.Arg1);
        }
    }

    public sealed class MethodInterceptionBindingWeaver : IFunctionBinding<CSharpDeveloperMixin, string, string>
    {
        public static MethodInterceptionBindingWeaver singleton = null;

        static MethodInterceptionBindingWeaver() {
            singleton = new MethodInterceptionBindingWeaver();
        }

        private MethodInterceptionBindingWeaver() {
        }

        public string Invoke(ref CSharpDeveloperMixin instance, IFunctionArgs<string, string> args) {
            var binding = MethodDecoratorFunctionBinding.singleton;
            //var aspectArgs = new FunctionInterceptionArgsImpl<CSharpDeveloperMixin, string, string>(instance, binding, args.Arg1);
            //Aspects.traceAspect3.OnEntry(aspectArgs);
            //Aspects.traceAspect3.OnSuccess(aspectArgs);

            var interceptionArgs = new FunctionInterceptionArgsImpl<CSharpDeveloperMixin, string, string>(instance, binding, args.Arg1);
            Aspects.traceAspect.OnInvoke(interceptionArgs);
            FunctionArgsMapper.Map<string, string>(interceptionArgs, args);

            return args.ReturnValue;
        }
    }

    internal static class FunctionArgsMapper
    {
        internal static void Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> first, IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.Arg3 = first.Arg3;
            second.Arg4 = first.Arg4;
            second.Arg5 = first.Arg5;
            second.Arg6 = first.Arg6;
            second.Arg7 = first.Arg7;
            second.Arg8 = first.Arg8;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult>(IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> first, IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.Arg3 = first.Arg3;
            second.Arg4 = first.Arg4;
            second.Arg5 = first.Arg5;
            second.Arg6 = first.Arg6;
            second.Arg7 = first.Arg7;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TResult>(IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> first, IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.Arg3 = first.Arg3;
            second.Arg4 = first.Arg4;
            second.Arg5 = first.Arg5;
            second.Arg6 = first.Arg6;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TArg2, TArg3, TArg4, TArg5, TResult>(IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> first, IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TArg5, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.Arg3 = first.Arg3;
            second.Arg4 = first.Arg4;
            second.Arg5 = first.Arg5;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TArg2, TArg3, TArg4, TResult>(IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TResult> first, IFunctionArgs<TArg1, TArg2, TArg3, TArg4, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.Arg3 = first.Arg3;
            second.Arg4 = first.Arg4;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TArg2, TArg3, TResult>(IFunctionArgs<TArg1, TArg2, TArg3, TResult> first, IFunctionArgs<TArg1, TArg2, TArg3, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.Arg3 = first.Arg3;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TArg2, TResult>(IFunctionArgs<TArg1, TArg2, TResult> first, IFunctionArgs<TArg1, TArg2, TResult> second) {
            second.Arg1 = first.Arg1;
            second.Arg2 = first.Arg2;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TArg1, TResult>(IFunctionArgs<TArg1, TResult> first, IFunctionArgs<TArg1, TResult> second) {
            second.Arg1 = first.Arg1;
            second.ReturnValue = first.ReturnValue;
        }

        public static void Map<TResult>(IFunctionArgs<TResult> first, IFunctionArgs<TResult> second) {
            second.ReturnValue = first.ReturnValue;
        }
    }

    public class Person : IPersonComposite
    {
        private CSharpDeveloperMixin developer = null;

        public Person() {
            developer = new CSharpDeveloperMixin();
        }

        public string Code(string sagi) {
            var aspectArgs = new FunctionExecutionArgsImpl<CSharpDeveloperMixin, string, string>(developer, sagi);
            Aspects.traceAspect3.OnEntry(aspectArgs);

            try {
                aspectArgs.ReturnValue = developer.Code(aspectArgs.Arg1);
                Aspects.traceAspect3.OnSuccess(aspectArgs);
            }
            catch (Exception ex) {
                aspectArgs.Exception = ex;
                Aspects.traceAspect3.OnException(aspectArgs);
            }
            finally {
                Aspects.traceAspect3.OnExit(aspectArgs);
            }

            return aspectArgs.ReturnValue;
        }

        public string Code2(string sagi) {
            var aspectArgs = new FunctionExecutionArgsImpl<CSharpDeveloperMixin, string, string>(developer, sagi);
            Aspects.traceAspect3.OnEntry(aspectArgs);

            try {
                aspectArgs.ReturnValue = developer.Code(aspectArgs.Arg1);
                Aspects.traceAspect3.OnSuccess(aspectArgs);
            }
            finally {
                Aspects.traceAspect3.OnExit(aspectArgs);
            }

            return aspectArgs.ReturnValue;
        }
    }

    [TransientComposite]
    [Mixins(typeof(CSharpDeveloperMixin))]
    public interface IPersonComposite : IDeveloper<ILanguage>
    {
        [OnMethodBoundaryAspect(typeof(TraceAspect3), AspectPriority = 1)]
        [OnMethodBoundaryAspect(typeof(TraceAspect3), AspectPriority = 1)]
        new string Code(string s);

    }

    class Program
    {
        static void Main(string[] args) {
            //new Person().Code(""); return;
            var container = new CompositeContainer();
            container.Configure();

            var person = container.TryResolve<IPersonComposite>();
            Console.WriteLine(person.Code("Sagi"));
        }
    }

    #region Composites

    public class GenericCovariantDeveloper<T> : IDeveloper<T>
        where T : ILanguage, new()
    {
        private T langugae = new T();

        public string Code(string s) {
            return langugae.ToString();
        }
    }

    public class TraceAspect : FunctionInterceptionAspect<string, string>
    {
        public override void OnInvoke(FunctionInterceptionArgs<string, string> args) {
            Console.WriteLine("Code from TraceAspect OnInvoke");
            base.OnInvoke(args);
        }
    }

    public class TraceAspect1 : FunctionInterceptionAspect<string>
    {
        public override void OnInvoke(FunctionInterceptionArgs<string> args) {
            base.OnInvoke(args);
        }
    }

    [PerThreadAspect]
    public class TraceAspect3 : OnFunctionBoundaryAspect<string, string>
    {
        public override void OnEntry(FunctionExecutionArgs<string, string> args) {
            Console.WriteLine("Code from TraceAspect3 OnEntry");
            base.OnEntry(args);
        }

        public override void OnExit(FunctionExecutionArgs<string, string> args) {
            Console.WriteLine("Code from TraceAspect3 OnExit");
            base.OnExit(args);
        }

        public override void OnException(FunctionExecutionArgs<string, string> args) {
            Console.WriteLine("Code from TraceAspect3 OnException");
            base.OnException(args);
        }

        public override void OnSuccess(FunctionExecutionArgs<string, string> args) {
            Console.WriteLine("Code from TraceAspect3 OnSuccess");
            base.OnSuccess(args);
        }
    }

    [PerThreadAspect]
    public class TraceAspect2 : OnFunctionBoundaryAspect<string>
    {
        public override void OnEntry(FunctionExecutionArgs<string> args) {
            base.OnEntry(args);
        }

        //public override void OnSuccess(FunctionExecutionArgs<string, bool> args) {
        //    base.OnSuccess(args);
        //}

        //public override void OnException(FunctionExecutionArgs<string, bool> args) {
        //    base.OnException(args);
        //}

        //public override void OnExit(FunctionExecutionArgs<string, bool> args) {
        //    base.OnExit(args);
        //}
    }

    public class CSharpDeveloperMixin : AbstractDeveloper<CSharpLanguage5>
    {
        public override string Code(string s) {
            throw new DivideByZeroException();
            return "Code";
        }
    }

    public class JavaScriptDeveloperMixin : AbstractDeveloper<JavaScriptLanguage>
    {

    }

    public abstract class AbstractDeveloper<TLanguage> : IDeveloper<TLanguage>
        where TLanguage : ILanguage, new()
    {
        private readonly ILanguage language = new TLanguage();

        public virtual string Code(string s) {
            return "I am coding in " + language.ToString();
        }
    }

    public interface ILanguage
    {
        string Description { get; }
    }

    public class CSharpLanguage : ILanguage
    {
        public virtual string Description {
            get {
                return "C#";
            }
        }
    }

    public class JavaScriptLanguage : ILanguage
    {
        public string Description {
            get {
                return "JavaScript";
            }
        }
    }

    public class CSharpLanguage5 : CSharpLanguage
    {
        public override string Description {
            get {
                return "C# 5";
            }
        }
    }

    public interface IDeveloper<out TLanguage>
    {
        string Code(string s);
    }

    public interface IDeveloper
    {
        void Code(string code);
    }

    #endregion Composites
}