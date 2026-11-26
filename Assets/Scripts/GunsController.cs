using System;
using UnityEngine;

public class GunsController : IInitializationModule, IUpdaterModule
{
    private bool animAct;
    private GunsData currentGun;

    public event Action<GunRef> ShotGunPlayerEvent;
    private GunsData[] All_guns_data;


    public GunsController(GunsData[] All_guns_data) 
    {;
        this.All_guns_data = All_guns_data;
    }

    public void Initialization()
    {
        for (int i = 0; i < All_guns_data.Length ; i++)
        {
            for (int j = 0; j < All_guns_data[i].parts_model.Length; j++)
            {
                All_guns_data[i].parts_model[j].enabled = false;
            }
        }

        currentGun = All_guns_data[0];
        MeshRenderControll(currentGun.parts_model, true);
    }

    public void UpdateMod(float delta)
    {
        if (InputMudule.instance.changeGunData && !animAct)
        {
            SwapGun();
        }

        // проверка нажатия атаки и отстутсвия других анимаций 
        if (InputMudule.instance.attackData && !animAct)
        {
            currentGun.particleFire.Play();
            currentGun.animator_model.SetTrigger("Shot");
            animAct = true;
            currentGun.startEndAnimations.EndAnim += AnimNoneActiv;
            ShotGunPlayerEvent?.Invoke(currentGun.configGun);
        }
    }
    // получение данных о том что анимация оружия уже прошла
    void AnimNoneActiv()
    {
        animAct = false;
        currentGun.startEndAnimations.EndAnim -= AnimNoneActiv;
    }

    private void SwapGun()
    {
        GunsArray[] allGun = (GunsArray[])Enum.GetValues(typeof(GunsArray));
        int index = (int)currentGun.type;
        GunsArray current = (GunsArray)((index + 1) % allGun.Length);

        for (int i = 0; i < All_guns_data.Length; i++)
        {
            if(All_guns_data[i].type == current)
            {
                MeshRenderControll(currentGun.parts_model, false);
                currentGun = All_guns_data[i];
                MeshRenderControll(currentGun.parts_model, true);
            }

        }
    }

    private void MeshRenderControll(MeshRenderer[] meshRenderers, bool mode)
    {
        if (mode)
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].enabled = false;
            }
        }
    }
}

[System.Serializable]
public class GunsData
{
    public MeshRenderer[] parts_model;
    public Animator animator_model;
    public GunsArray type;
    public ParticleSystem particleFire;
    public StartEndAnimations startEndAnimations;
    public GunRef configGun;

    public GunsData(MeshRenderer[] parts_model,
        Animator animator_model, GunsArray type,
        ParticleSystem particleFire, StartEndAnimations startEndAnimations,
        GunRef configGun)
    {
        this.parts_model = parts_model;
        this.animator_model = animator_model;
        this.type = type;
        this.particleFire = particleFire;
        this.startEndAnimations = startEndAnimations;
        this.configGun = configGun;
    }
}

public enum GunsArray {
    shotgun = 0,
    automatic_rifle = 1,
    handgun = 2
}
