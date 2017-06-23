/****************************************************************************
 * Copyright (c) 2016 qianmo
 * Copyright (c) 2017 liangxie
 *
 * https://github.com/QianMo/Unity-Design-Pattern/blob/master/Assets/Creational%20Patterns/Builder%20Pattern/Structure/BuilderStructure.cs
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example {

	/// <summary>
	/// 建造者模式
	/// </summary>
	public class PatternBuilder : MonoBehaviour {

		void Start()
		{
			// Create director and builders
			Director director = new Director();

			Builder builder1 = new ConcreteBuilder1 ();
			Builder builder2 = new ConcreteBuilder2 ();

			// Construct two products
			director.Construct (builder1);
			Product product1 = builder1.GetResult ();
			product1.Show ();

			director.Construct (builder2);
			Product product2 = builder2.GetResult ();
			product2.Show ();
		}
	}

	/// <summary>
	/// The 'Director' class
	/// </summary>
	class Director
	{
		// Builder uses a complex series of steps
		public void Construct(Builder builder)
		{
			builder.BuildPartA();
			builder.BuildPartB();
		}
	}
		
	/// <summary>
	/// The 'Builder' abstract class
	/// </summary>
	abstract class Builder
	{
		public abstract void BuildPartA();

		public abstract void BuildPartB();

		public abstract Product GetResult();
	}

	/// <summary>
	/// The 'ConcreteBuilder1' class
	/// </summary>
	class ConcreteBuilder1 : Builder
	{
		private Product mProduct = new Product();

		public override void BuildPartA()
		{
			mProduct.Add("PartA");
		}

		public override void BuildPartB()
		{
			mProduct.Add("PartB");
		}

		public override Product GetResult()
		{
			return mProduct;
		}
	}


	/// <summary>
	/// The 'ConcreteBuilder2' class
	/// </summary>
	class ConcreteBuilder2 : Builder
	{
		private Product mProduct = new Product();

		public override void BuildPartA()
		{
			mProduct.Add("PartX");
		}

		public override void BuildPartB()
		{
			mProduct.Add("PartY");
		}

		public override Product GetResult()
		{
			return mProduct;
		}
	}


	/// <summary>
	/// The 'product' class
	/// </summary>
	class Product {
		
		private List<string> mParts = new List<string>();

		public void Add(string part) 
		{
			mParts.Add (part);
		}

		public void Show() 
		{
			Debug.Log ("\nProduct Parts -------");
			foreach (string part in mParts) {
				Debug.Log (part);
			}
		}
	}
}