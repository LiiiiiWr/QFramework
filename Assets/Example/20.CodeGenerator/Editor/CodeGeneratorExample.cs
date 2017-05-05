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
 * 
****************************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using QFramework.Libs;

namespace QFramework.Example.Editor {
	/// <summary>
	/// CodeGenerator示例
	/// </summary>
	public class CodeGeneratorExample  {

		[MenuItem("QFramework/Example/20.GenClassUsingCodeDom")]
		static void GenClassUsingCodeDom() {
			
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit ();
			CodeNamespace codeNamespace = new CodeNamespace ("QFramework.Example");
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration ("ExampleClass");

			codeCompileUnit.Namespaces.Add (codeNamespace);
			codeNamespace.Types.Add (codeTypeDeclaration);

			var provider = new CSharpCodeProvider ();
			var options = new CodeGeneratorOptions ();
			var genFilePath = Application.dataPath + "/Example/20.CodeGenerator/ExampleClass.cs";

			StreamWriter writer = new StreamWriter(File.Open (genFilePath, FileMode.Create));

			provider.GenerateCodeFromCompileUnit (codeCompileUnit, writer, options);
			writer.Close ();
			AssetDatabase.Refresh ();
		}

		[MenuItem("QFramework/Example/20.GenClassUsingCodeGenerator")]
		static void GenClassUsingCodeGenerator() {

			QNamespaceDefine namespaceDefine = new QNamespaceDefine ();

			namespaceDefine.Name = "QFramework.Example";
			namespaceDefine.FileName = "ExampleClass.cs";
			namespaceDefine.GenerateDir = Application.dataPath + "/Example/20.CodeGenerator/";

			QClassDefine classDefine = new QClassDefine ();
			classDefine.Name = "ExampleClass";

			namespaceDefine.Classes.Add (classDefine);

			QCodeGenerator.Generate (namespaceDefine);
		}

		[MenuItem("QFramework/Example/20.QCodeGeneratorExample")]
		static void QCodeGeneratorExample() {

			//定义命名空间
			QNamespaceDefine namespaceDefine = new QNamespaceDefine ();

			namespaceDefine.Name = "QFramework.Example";
			namespaceDefine.FileName = "QExampleClass.cs";
			namespaceDefine.GenerateDir = Application.dataPath + "/Example/20.CodeGenerator/";

			//定义类
			QClassDefine classDefine = new QClassDefine ();
			classDefine.Name = "QExampleClass";
			namespaceDefine.Classes.Add (classDefine);

			// 定义变量
			var variable = new QVariable ();
			variable.AccessLimit = QAccessLimit.Private;
			variable.CompileType = QCompileType.Const;
			variable.Type = TypeUtil.GetCodeType (QTypeDefine.String);
			variable.Name = "mVariable";
			variable.Value = "Hello";
			classDefine.Variables.Add (variable);

			// 定义属性
			var property = new Property ();
			property.AccessLimit = QAccessLimit.Public;
			property.CompileType = QCompileType.Static;
			property.Type = TypeUtil.GetCodeType (QTypeDefine.String);
			property.Name = "Variable";
			property.GetReturnCode = "mVariable";
			classDefine.Properties.Add (property);

			// 生成
			QCodeGenerator.Generate (namespaceDefine);
		}
	}
}
