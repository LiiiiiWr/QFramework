using System;
using QFramework;

namespace QFramework
{
	public partial class QUIFactory : ISingleton
	{
		private QUIFactory() {}

		public static QUIFactory Instance {
			get {
				return QSingletonProperty<QUIFactory>.Instance;
			}
		}

		public void OnSingletonInit()
		{

		}

		public static void Dispose()
		{
			QSingletonProperty<QUIFactory>.Dispose ();
		}
		public IUIComponents CreateUIComponents(string uiName)
		{
			return CreateUIComponentsByUIName(uiName);
		}
	}
}
