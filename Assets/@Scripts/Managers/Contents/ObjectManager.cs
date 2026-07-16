using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class ObjectManager
{
    //public HashSet<Hero> Heroes { get; } = new HashSet<Hero>();
    public Hero Player { get; private set; }
    public Boss Boss { get; private set; }
    public HashSet<Monster> Monsters { get; } = new HashSet<Monster>();
    public HashSet<Artifact> Artifacts { get; } = new HashSet<Artifact>();
    //public HashSet<InteractableObject> Interactions { get; } = new HashSet<InteractableObject>();
    //public HashSet<Item> Items { get; } = new HashSet<Item>();


    #region Roots
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }
    
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform BossRoot { get { return GetRootTransform("@Bosses"); } }
    public Transform ArtifactRoot { get { return GetRootTransform("@Artifacts"); } }
    public Transform ItemRoot { get { return GetRootTransform("@Items"); } }
    public Transform InteractionRoot { get { return GetRootTransform("@Interactions"); } }
    #endregion

    public T Spawn<T>(Vector3 position, int templateID) where T : BaseObject
    {
        string prefabName = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate(prefabName);
        go.name = prefabName;
        go.transform.position = position;

        BaseObject obj = go.GetComponent<BaseObject>();

        if (obj.ObjectType == EObjectType.Creature)
        {
            Creature creature = go.GetComponent<Creature>();
            switch (creature.CreatureType)
            {
                case ECreatureType.Hero:
                    Debug.Log("Hero Spawn");
                    Hero hero = creature as Hero;
                    Player = hero;
                    hero.InitVector = position;
                    break;
                case ECreatureType.Monster:
                    Debug.Log("Monster Spawn");
                    obj.transform.parent = MonsterRoot;
                    Monster monster = creature as Monster;
                    Monsters.Add(monster);
                    monster.InitVector = position;
                    break;
                case ECreatureType.Boss:
                    Debug.Log("Boss Spawn");
                    obj.transform.parent = BossRoot;
                    Boss boss = creature as Boss;
                    Boss = boss;
                    boss.InitVector = position;
                    break;
                case ECreatureType.Artifact:
                    Debug.Log("Artifact Spawn");
                    obj.transform.parent = ArtifactRoot;
                    Artifact artifact = creature as Artifact;
                    Artifacts.Add(artifact);
                    break;
            }

            creature.SetInfo(templateID);
        }
        else if (obj.ObjectType == EObjectType.Interaction)
        {
            obj.transform.parent = InteractionRoot;
            //InteractableObject interaction = go.GetComponent<InteractableObject>();
            //Interactions.Add(interaction);
            //interaction.SetInfo(templateID);
        }
        else if (obj.ObjectType == EObjectType.Item)
        {
            obj.transform.parent = ItemRoot;
            //Item item = go.GetComponent<Item>();
            //Items.Add(item);
            //item.SetInfo(templateID);
        }

        return obj as T;
    }

    public void Despawn<T>(T obj) where T : BaseObject
    {
        if (obj == null)
            return;

        EObjectType objectType = obj.ObjectType;

        if (obj.ObjectType == EObjectType.Creature)
        {
            Creature creature = obj.GetComponent<Creature>();
            switch (creature.CreatureType)
            {
                case ECreatureType.Hero:
                    Hero hero = creature as Hero;
                    Player = null;
                    break;
                case ECreatureType.Monster:
                    Monster monster = creature as Monster;
                    Monsters.Remove(monster);
                    break;
                case ECreatureType.Boss:
                    Boss boss = creature as Boss;
                    Boss = null;
                    break;
                case ECreatureType.Artifact:
                    Artifact artifact = creature as Artifact;
                    Artifacts.Remove(artifact);
                    break;
            }
        }
        else if (obj.ObjectType == EObjectType.Interaction)
        {
            
        }
        else if (obj.ObjectType == EObjectType.Item)
        {
            
        }

        Managers.Resource.Destroy(obj.gameObject);
        Debug.Log($"{obj} is Despawned");
    }
}
