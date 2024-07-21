using UnityEngine;

public abstract class Entity: ITalk
{
    public abstract void Talk();
    public abstract void Listen();
}

public class Dog : Entity
{
    public override void Talk()
    {
        Debug.Log($"I am dog");
    }

    public override void Listen()
    {
        throw new System.NotImplementedException();
    }
}

public class Cat : Entity
{
    public override void Talk()
    {
        Debug.Log($"I am cat");
    }

    public override void Listen()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        
    }
}

public class Human: ITalk
{
    public void Talk()
    {
        Debug.Log($"I am human");
    }
}

public interface ITalk
{
    public void Talk();
}