using UnityEngine;

namespace WaterNavTiled
{
    public abstract class Layer : MonoBehaviour
    {
        [SerializeField]protected string Classname;
        [SerializeField]protected int Id; //Readonly??
        [SerializeField]protected bool Locked;
        [SerializeField]protected string Name;
        [SerializeField] protected float Opacity;
        //Array of properties??
        [SerializeField] protected string Type; //Could replace with an enum for Tilelayer, ObjectGrouplayer, ImageLayer or just group
        [SerializeField] protected bool Visable;
    }
}