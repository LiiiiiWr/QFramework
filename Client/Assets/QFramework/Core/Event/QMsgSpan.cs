/****************************************************************************
 * Copyright (c) 2017 xiaojun
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
 * 
 ****************************************************************************/
namespace QFramework
{
	public class QMsgSpan 
	{
		public const int Count = 3000;
	}

	public partial class QMgrID
	{
		public const int Framework = QMsgSpan.Count * 0;
		public const int UI = QMsgSpan.Count; 				// 3000
		public const int Audio = QMsgSpan.Count * 2;		// 6000
		public const int Network = QMsgSpan.Count * 3;
		public const int UIFilter = QMsgSpan.Count * 4;
		public const int Game = QMsgSpan.Count * 5;
		public const int PCConnectMobile = QMsgSpan.Count * 6;

		public const int FrameworkMsgModuleCount = 6;
	}
}