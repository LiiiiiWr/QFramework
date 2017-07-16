/****************************************************************************
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
****************************************************************************/

namespace QFramework 
{
	public partial class QMgrID
	{
		public const int AB = QMsgSpan.Count * (FrameworkMsgModuleCount + 1);
		public const int AR = QMsgSpan.Count * (FrameworkMsgModuleCount + 2);
		public const int Letter = QMsgSpan.Count * (FrameworkMsgModuleCount + 3);
		public const int Speech = QMsgSpan.Count * (FrameworkMsgModuleCount + 4);
		public const int Data = QMsgSpan.Count * (FrameworkMsgModuleCount + 5);
	}
	
	public partial class QMsgCenter  
	{
		/// <summary>
		/// 转发消息
		/// </summary>
		private void ForwardMsg(QMsg msg)
		{
			int tmpId = msg.GetMgrID();

			switch (tmpId)
			{
				case  QMgrID.UI:
					QUIManager.Instance.SendMsg (msg);
					break;
			}
		}
	}
}