﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Autofac.Core;

namespace Autofac.Test
{
    public class TypedParameterTests
    {
        public class A
        {
        }

        public class B : A { }

        public class C
        {
            public C(A a)
            {
            }
        }

        [Fact]
        public void MatchesIdenticallyTypedParameter()
        {
            var param = AParamOfCConstructor();

            var typedParam = new TypedParameter(typeof(A), new A());

            Func<object> vp;
            Assert.True(typedParam.CanSupplyValue(param, new Container(), out vp));
        }

        private static System.Reflection.ParameterInfo AParamOfCConstructor()
        {
            var param = typeof(C)
                .GetConstructor(new Type[] { typeof(A) })
                .GetParameters()
                .First();
            return param;
        }

        [Fact]
        public void DoesNotMatchPolymorphicallyTypedParameter()
        {
            var param = AParamOfCConstructor();

            var typedParam = new TypedParameter(typeof(B), new B());

            Func<object> vp;
            Assert.False(typedParam.CanSupplyValue(param, new Container(), out vp));
        }

        [Fact]
        public void DoesNotMatchUnrelatedParameter()
        {
            var param = AParamOfCConstructor();

            var typedParam = new TypedParameter(typeof(string), "Yo!");

            Func<object> vp;
            Assert.False(typedParam.CanSupplyValue(param, new Container(), out vp));
        }

		[Fact]
		public void FromWorksJustLikeTheConstructor()
		{
			var param = TypedParameter.From(new B());
			Assert.Same(typeof(B), param.Type);
		}
    }
}