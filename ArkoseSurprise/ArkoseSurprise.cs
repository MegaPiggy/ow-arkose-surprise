using HarmonyLib;
using OWML.ModHelper;
using System.Reflection;

namespace ArkoseSurprise;

[HarmonyPatch]
public class ArkoseSurprise : ModBehaviour
{
	private static ArkoseSurprise Instance;

	private void Awake()
	{
		Instance = this;
		Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
	}

	[HarmonyPostfix, HarmonyPatch(typeof(KidRockController), nameof(KidRockController.Start))]
	private static void KidRockController_Start(KidRockController __instance)
	{
		Instance.ModHelper.Console.WriteLine("arkose hook");
		var shape = __instance._rockCollider.gameObject.AddComponent<BoxShape>();
		shape.CopySettingsFromCollider();
		shape.OnCollisionEnter += otherShape =>
		{
			Instance.ModHelper.Console.WriteLine($"hit {otherShape}");
			if (otherShape.GetAttachedOWRigidbody().CompareTag("Player"))
			{
				Instance.ModHelper.Console.WriteLine("kill");
				Locator.GetDeathManager().KillPlayer(DeathType.Impact);
			}
		};
	}
}
