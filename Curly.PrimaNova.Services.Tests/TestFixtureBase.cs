using System;
using Autofac.Extras.Moq;
using Moq;

namespace Curly.PrimaNova.Services.Tests
{
	public class TestFixtureBase<T> : IDisposable
	{
		protected AutoMock AutoMock { get; }

		protected TestFixtureBase()
		{
			AutoMock = AutoMock.GetLoose();
		}

		protected T GetSubject()
		{
			return AutoMock.Create<T>();
		}

		protected Mock<TMock> GetMock<TMock>() where TMock : class
		{
			return AutoMock.Mock<TMock>();
		}

		public void Dispose()
		{
			AutoMock?.Dispose();
		}
	}
}