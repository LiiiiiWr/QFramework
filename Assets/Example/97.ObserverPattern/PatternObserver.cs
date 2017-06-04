/****************************************************************************
 * Copyright (c) 2016 qianmo
 * Copyright (c) 2017 liangxie
 *
 * https://github.com/QianMo/Unity-Design-Pattern/blob/master/Assets/Behavioral%20Patterns/Observer%20Pattern/Structure/ObserverStructure.cs
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

using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public class PatternObserver : MonoBehaviour
    {
        void Start()
        {
            // Configure Observer pattern
            ConcreteSubject concreteSubject = new ConcreteSubject();
            
            concreteSubject.Attach(new ConcreteObserver(concreteSubject,"X"));
            concreteSubject.Attach(new ConcreteObserver(concreteSubject,"Y"));
            concreteSubject.Attach(new ConcreteObserver(concreteSubject,"Z"));
        
            // Change subject and notify observers
            concreteSubject.SubjectState = "ABC";
            concreteSubject.Notify();
            
            // Change subject and notify observers again
            concreteSubject.SubjectState = "666";
            concreteSubject.Notify();
         
        }
    }

    abstract class Subject
    {
        private List<Observer> mObservers = new List<Observer>();

        public void Attach(Observer observer)
        {
            mObservers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            mObservers.Remove(observer);
        }

        public void Notify()
        {
            foreach (Observer observer in mObservers)
            {
                observer.Update();
            }
        }
    }

    /// <summary>
    /// The 'ConcreteSubject' class
    /// </summary>
    class ConcreteSubject : Subject
    {
        private string mSubjectState;
        
        // Gets or sets subject state
        public string SubjectState
        {
            get { return mSubjectState; }
            set { mSubjectState = value; }
        }
    }
    
    /// <summary>
    /// The 'Observer' abstract class
    /// </summary>
    abstract class Observer
    {
        public abstract void Update();
    }

    class ConcreteObserver : Observer
    {
        private string mName;
        private string mObserverState;
        private ConcreteSubject mSubject;

        public ConcreteObserver(ConcreteSubject subject, string name)
        {
            mSubject = subject;
            mName = name;
        }

        public override void Update()
        {
            mObserverState = mSubject.SubjectState;
            Debug.Log("Observer " + mName + "'s new state is " + mObserverState);
        }
        
        // Gets or sets subject
        public ConcreteSubject Subject
        {
            get { return mSubject; }
            set { mSubject = value; }
        }
    }
}