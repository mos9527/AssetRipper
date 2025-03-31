using System.Text.RegularExpressions;

namespace AssetRipper.Export.UnityProjects.Scripts;

// GUID, Version Min, Version Max (non-inclusive)
using UnityExtensionAssembly = Tuple<UnityGuid, UnityVersion, UnityVersion>;
public static partial class ReferenceAssemblies
{
	[GeneratedRegex(@"^Unity(\.[0-9a-zA-Z]+)*(\.dll)?$")]
	private static partial Regex UnityRegex { get; }

	[GeneratedRegex(@"^UnityEngine(\.[0-9a-zA-Z]+)*(\.dll)?$")]
	private static partial Regex UnityEngineRegex { get; }

	[GeneratedRegex(@"^UnityEditor(\.[0-9a-zA-Z]+)*(\.dll)?$")]
	private static partial Regex UnityEditorRegex { get; }

	[GeneratedRegex(@"^System(\.[0-9a-zA-Z]+)*(\.dll)?$")]
	private static partial Regex SystemRegex { get; }

	private static HashSet<string> WhitelistAssemblies { get; } =
	[
		"UnityEngine.UI.dll",
		"UnityEngine.UI",
	];

	private static HashSet<string> BlacklistAssemblies { get; } =
	[
		"mscorlib.dll",
		"mscorlib",
		"netstandard.dll",
		"netstandard",
		"Mono.Security.dll",
		"Mono.Security"
	];

	private static HashSet<string> PredefinedAssemblies { get; } =
	[
		"Assembly-CSharp.dll",
		"Assembly-CSharp",
		"Assembly-CSharp-firstpass.dll",
		"Assembly-CSharp-firstpass",
		"Assembly-CSharp-Editor.dll",
		"Assembly-CSharp-Editor",
		"Assembly-CSharp-Editor-firstpass.dll",
		"Assembly-CSharp-Editor-firstpass",
		"Assembly-UnityScript.dll",
		"Assembly-UnityScript",
		"Assembly-UnityScript-firstpass.dll",
		"Assembly-UnityScript-firstpass"
	];

	private static readonly Dictionary<string, UnityExtensionAssembly> UnityExtensionAssemblies =
		new Dictionary<string, UnityExtensionAssembly>() {
			{"UI",new UnityExtensionAssembly(new UnityGuid(0x25C76F5F, 0x4FD4651D, 0xDCC6398A, 0x8DB3A202), new UnityVersion(5), new UnityVersion(2019))},
			{"Networking",new UnityExtensionAssembly(new UnityGuid(0x98353078, 0x2E043BB1, 0x078C9A2B, 0xAB9147E7), new UnityVersion(5), new UnityVersion(2019))},
			{"PerformanceTesting",new UnityExtensionAssembly(new UnityGuid(0xBB836ED0, 0x8024F66B, 0xAE06BFBA, 0x423040E8), new UnityVersion(2017), new UnityVersion(2018))},
			{"TestRunner",new UnityExtensionAssembly(new UnityGuid(0xAAFCBE35, 0xD2E4E1E2, 0xC28858CB, 0x1AF37A5D), new UnityVersion(5), new UnityVersion(2019))},
			{"Timeline",new UnityExtensionAssembly(new UnityGuid(0x092B01A6, 0xF7843829, 0x9D00B319, 0x5FAF3DC4), new UnityVersion(2017), new UnityVersion(2019))},
			{"UIAutomation",new UnityExtensionAssembly(new UnityGuid(0xCBCA867C, 0x93D42FC9, 0xC4BB8848, 0x88219B39), new UnityVersion(2017), new UnityVersion(2018))},
			{"GoogleAudioSpatializer",new UnityExtensionAssembly(new UnityGuid(0xB1FC4F4E, 0x731434B9, 0xA309994A, 0xA156D1A7), new UnityVersion(2017), new UnityVersion(2019))},
			{"HoloLens",new UnityExtensionAssembly(new UnityGuid(0x4FF45B7F, 0xFCF4D34A, 0xB8354B18, 0xCCB0E876), new UnityVersion(5), new UnityVersion(2018))},
			{"SpatialTracking",new UnityExtensionAssembly(new UnityGuid(0x3F3437DE, 0x3B3483E0, 0xB8F8ADFA, 0xAEC96620), new UnityVersion(2017), new UnityVersion(2019))},
			{"Advertisements",new UnityExtensionAssembly(new UnityGuid(0xF9DBB937, 0x8624B463, 0x8DF9F478, 0xFEEB3BA6), new UnityVersion(5), new UnityVersion(6))},
			{"Analytics",new UnityExtensionAssembly(new UnityGuid(0x0865E258, 0x36149BE2, 0x194BBCA8, 0x0B794418), new UnityVersion(5), new UnityVersion(6))},
			{"Purchasing",new UnityExtensionAssembly(new UnityGuid(0xDE8DC0E8, 0xC2144D44, 0x602460EB, 0x44E9CBA7), new UnityVersion(5), new UnityVersion(6))},
			{"VR",new UnityExtensionAssembly(new UnityGuid(0xC5E1FDC6, 0x02741D87, 0x4DCCDAAA, 0x69FD297C), new UnityVersion(5), new UnityVersion(6))},
		};

	public static UnityGuid GetUnityExtensionAssemblyGuid(string namespaceName, string name)
	{
		if (!namespaceName.StartsWith("Unity") || !UnityExtensionAssemblies.ContainsKey(name))
		{
			throw new ArgumentException($"Assembly {namespaceName}.{name} is not an Unity extension assembly");
		}
		return UnityExtensionAssemblies[name].Item1;
	}

	public static UnityGuid GetUnityExtensionAssemblyGuid(string assemblyName)
	{
		string[] name = assemblyName.Split('.');
		if (name.Length < 2)
		{
			throw new ArgumentException($"Assembly {assemblyName} is not an Unity extension assembly");
		}
		return GetUnityExtensionAssemblyGuid(name[0], name[1]);
	}
	public static bool IsUnityExtensionAssembly(string namespaceName, string name, UnityVersion version)
	{
		if (!namespaceName.StartsWith("Unity")) return false;
		if (!UnityExtensionAssemblies.ContainsKey(name))
		{
			return false;
		}
		UnityExtensionAssembly assembly = UnityExtensionAssemblies[name];
		if (version < assembly.Item2 || version >= assembly.Item3)
		{
			return false;
		}
		return true;
	}

	public static bool IsUnityExtensionAssembly(string assemblyName, UnityVersion version)
	{
		string[] name = assemblyName.Split('.');
		return name.Length >= 2 && IsUnityExtensionAssembly(name[0], name[1], version);
	}
	public static bool IsPredefinedAssembly(string assemblyName)
	{
		ArgumentNullException.ThrowIfNull(assemblyName);
		return PredefinedAssemblies.Contains(assemblyName);
	}

	public static bool IsReferenceAssembly(string assemblyName)
	{
		ArgumentNullException.ThrowIfNull(assemblyName);

		if (IsWhiteListAssembly(assemblyName))
		{
			return false;
		}

		return IsBlackListAssembly(assemblyName)
			|| IsUnityEngineAssembly(assemblyName)
			//|| IsUnityAssembly(assemblyName)
			|| IsSystemAssembly(assemblyName)
			|| IsUnityEditorAssembly(assemblyName);
	}

	private static bool IsUnityAssembly(string assemblyName) => UnityRegex.IsMatch(assemblyName);
	public static bool IsUnityEngineAssembly(string assemblyName) => UnityEngineRegex.IsMatch(assemblyName);
	private static bool IsUnityEditorAssembly(string assemblyName) => UnityEditorRegex.IsMatch(assemblyName);
	private static bool IsSystemAssembly(string assemblyName) => SystemRegex.IsMatch(assemblyName);
	private static bool IsWhiteListAssembly(string assemblyName) => WhitelistAssemblies.Contains(assemblyName);
	private static bool IsBlackListAssembly(string assemblyName) => BlacklistAssemblies.Contains(assemblyName);
}
