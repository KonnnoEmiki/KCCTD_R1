#if UNITY_EDITOR
// ScriptableObjectのスクリプト動的生成用のテンプレートコード
public static class ScriptableObjectDefines
{
	public const string TemplateScript =
@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class #name# : ScriptableObject
{
	
}
";

	public const string TemplateScript_NonScope =
@"using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class #name# : ScriptableObject
";

}
#endif