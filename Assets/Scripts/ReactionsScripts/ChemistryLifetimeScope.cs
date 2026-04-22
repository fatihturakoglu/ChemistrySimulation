using VContainer;
using VContainer.Unity;
using UnityEngine;

public class ChemistryLifetimeScope : LifetimeScope
{
    [SerializeField] private BeakerManager beaker;
    [SerializeField] private SelectionManager selection;

    protected override void Configure(IContainerBuilder builder)
    {
        // 1. Saf C# Servisleri
        builder.Register<ColorMixerService>(Lifetime.Singleton);

        // 2. Sahnedeki Objeler
        builder.RegisterComponent(selection);

        // BeakerManager'ı hem kendisi hem de IBeaker interface'i olarak tanıtıyoruz
        builder.RegisterComponent(beaker).AsImplementedInterfaces().AsSelf();
    }
}