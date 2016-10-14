using UnityEngine;
using System.Collections;

public class Sample : MonoBehaviour {

	// Use this for initialization
	public string ChartObjectName;
	DynamicData dyData;
	void Start () {

		//the workflow as follow
		//use AddSeries() add series
		//Use Render () to Render chart
		//Use Next() method to response User's action to show data
		//in this example, when i press any key or mouse, it will add show one data
		//you can use ShowAll() to ingore those steps

		this.dyData = GameObject.Find (ChartObjectName).GetComponent<DynamicData> ();
		if (this.dyData != null) {
		
			int seriesCount = 5;
			int serPointsCount = Random.Range (3, 6);
			for (int i = 0; i < seriesCount; i++) {
			
				ChartType cType = Random.Range (0f, 1.0f) > 0.5 ? ChartType.Bar : ChartType.Line;
				Series ser = new Series ("series" + i.ToString(), cType);

				ser.Points = new float[serPointsCount];
				for (int j = 0; j < serPointsCount; j++) {
					ser.Points [j] = Random.Range (20.0f, 40.0f);
				}

				this.dyData.AddSeries (ser);
			}

			this.dyData.Render ();
			//this.dyData.ShowAll ();
		}


	}
	
	// Update is called once per frame
	void Update () {
		if(this.dyData!=null && Input.anyKeyDown){
			this.dyData.Next ();
		}
	}
}
