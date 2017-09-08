/****************************************************************************
 * Copyright (c) 2017 snowcold
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

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class TimerTest : MonoBehaviour
    {
        //private TimeItem m_RepeatTimeItem;

        private void Start()
        {
            Log.i(DateTime.Now);
            //m_RepeatTimeItem = Timer.S.Post2Really(OnTimeTick, 1, -1);
            //DateTime time = DateTime.Now;
            //time = time.AddSeconds(5);
            //Timer.S.Post2Really(OnDateTimeTick, time);

            //Time.timeScale = 0.5f;
            //Timer.S.Post2Scale(OnScaleTimeTick, 1, -1);
        }

        private void OnTimeTick(int tick)
        {
            Log.i("TickTick:" + DateTime.Now);
        }

        private void OnDateTimeTick(int tick)
        {
            Log.i("DateTimeTick:" + tick);
        }

        private void OnScaleTimeTick(int tick)
        {
            Log.i("ScaleTickTick:" + tick);
        }
    }
}
