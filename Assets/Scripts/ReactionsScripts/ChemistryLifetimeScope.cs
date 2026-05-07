using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ChemistryLifetimeScope : LifetimeScope
{
    [SerializeField] private BeakerManager beaker;
    [SerializeField] private SelectionManager selection;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<ColorMixerService>(Lifetime.Singleton);
        builder.RegisterComponent(selection);
        builder.RegisterComponent(beaker).AsImplementedInterfaces().AsSelf();
    }
}