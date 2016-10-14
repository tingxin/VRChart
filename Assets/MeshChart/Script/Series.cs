using UnityEngine;
using System.Collections;

public enum ChartType{
	Bar,
	Line
}

public class Series{

	public Series(string name, ChartType seriesType){

		this.Name = name;
		this.SeiresType = seriesType;
	}
	public string Name{ get; private set;}
	public ChartType SeiresType;
	public float[] Points;

	public override bool Equals (object obj)
	{
		Series target = obj as Series;
		if (target != null) {
			if (target.Name == this.Name) {
				return true;
			}
		}
		return false;
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
}

