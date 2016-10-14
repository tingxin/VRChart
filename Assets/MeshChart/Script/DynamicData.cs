using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicData : MonoBehaviour {

	public BarChart ManualBarChart;
	public LineChart ManualLineChart;
	public BarChart AutoBarChart = null; 
	public LineChart AutoLineChart = null;

	private List<Series> dataSource = new List<Series> ();

	private bool isBusy = false;
	private int MaxDataLength = 0;

	private float[][] manualDataCache;
	private ChartType[] seriesTypeCache;
	private float[][] barData;
	private float[][] lineData;

	private int controlStep = 0;
	private int controlStepPre = 0;
	private bool nextIsWork = true;
	// Use this for initialization


	private float t; 

	private float[][] dataAuto; 
 

	void Start () {

		this.InitDataCache ();


	}
	
	// Update is called once per frame
	void Update () {

		this.UpdateAutoChart ();

	}

	public void Next(){
		this.controlStep++;
	}

	public void ShowAll(){
		this.nextIsWork = false;
	}

	public void ClearSeries(){
		this.dataSource.Clear ();
	}

	public void RemoveSeries(string seriesName){
		Series target = null;
		for (int i = 0; i < this.dataSource.Count; i++) {
			if (this.dataSource [i].Name == seriesName) {
				target = this.dataSource [i];
				break;
			}
		}

		if (target!=null) {
			this.dataSource.Remove (target);
		} else {
			print (string.Format ("can not find the series: %s, because don't have the this series yet", seriesName));
		}
	}

	public void AddSeries(Series series){
		if (this.dataSource.Contains (series) == false) {
			this.dataSource.Add (series);
		} else {
			print (string.Format ("can not add the series: %s, because have the this series yet", series.Name));
		}
	}

	public void Render(){
		this.isBusy = true;
		this.MaxDataLength = 0;
		for (int i = 0; i < this.dataSource.Count; i++) {
			if (this.dataSource [i].Points.Length > this.MaxDataLength) {
				this.MaxDataLength = this.dataSource [i].Points.Length;
			}
		}

		int series = this.dataSource.Count;
		this.manualDataCache = new float[series][];
		this.seriesTypeCache = new ChartType[series];
		int lineCount = 0;
		int barCount = 0;
		for (int i = 0; i < series; i++) {
			Series currentSeries = this.dataSource [i];
			this.manualDataCache[i] = new float[this.MaxDataLength];
			this.seriesTypeCache [i] = currentSeries.SeiresType;

			if (currentSeries.SeiresType == ChartType.Bar) {
				barCount++;
			} else if (currentSeries.SeiresType == ChartType.Line) {
				lineCount++;
			}

			for (int j = 0; j < this.MaxDataLength; j++) {

				if (j < currentSeries.Points.Length) {
					this.manualDataCache [i] [j] = currentSeries.Points [j];
				} else {
					this.manualDataCache [i] [j] = 0;
				}
			}
		}

		this.barData = new float[barCount][];
		this.lineData = new float[lineCount][];
		for (int i = 0; i < barCount; i++) {
			this.barData [i] = new float[this.MaxDataLength];
		}
		for (int i = 0; i < lineCount; i++) {
			this.lineData [i] = new float[this.MaxDataLength];
		}
		this.isBusy = false;
		this.controlStepPre = 0;
		this.controlStep = 0;
		this.nextIsWork = true;
		StartCoroutine ("UpdateManualChart");
	}

	void InitDataCache(){
		int max = 5; 
		t = 0; 
		this.dataAuto = new float[3][]; 
		for(int i=0;i<dataAuto.Length;i++) {
			this.dataAuto[i] = new float[max]; 
			for(int j=0;j<max;j++) {
				this.dataAuto[i][j] = Mathf.Sin(i+(float)j / (float)max * 2.0f * Mathf.PI); 
			}
		}
		float[][] empty = new float[2][];
		for (int i = 0; i < 2; i++) {
			empty [i] = new float[2];
			for (int j = 0; j < 2; j++) {
				empty [i] [j] = 0;
			}
		}
		this.ManualBarChart.UpdateData (empty);
		this.ManualLineChart.UpdateData (empty);
	}

	void UpdateAutoChart(){
		t += 1.0f; 

		for(int i=0;i<this.dataAuto.Length;i++) {
			int max = this.dataAuto[i].Length; 
			for(int j=0;j<max;j++) {
				this.dataAuto[i][j] = Mathf.Sin(t * 0.01f + i+(float)j / (float)max * 2.0f * Mathf.PI); 
			}
		}

		if(AutoBarChart != null) {
			AutoBarChart.UpdateData(this.dataAuto); 
		}
		if(AutoLineChart != null) {
			AutoLineChart.UpdateData(dataAuto); 
		}


		for(int i=0;i<this.dataAuto.Length;i++) {
			int max = this.dataAuto[i].Length; 
			for(int j=0;j<max;j++) {
				this.dataAuto[i][j] = Mathf.Abs(Mathf.Sin(t * 0.01f + i+(float)j / (float)max * 2.0f * Mathf.PI)); // value in Pie/Doughnut chart must be positive value.  
			}
		}
	}

	IEnumerator UpdateManualChart(){
		int i = 0;
		int j = 0;
		int lineIndex = 0;
		int barIndex = 0;
		while (this.isBusy == false) {
			if (this.controlStep!=this.controlStepPre|| this.nextIsWork == false) {
				this.controlStepPre++;
				float value = 0;
				ChartType chartType = this.seriesTypeCache [i];

				while (value < this.manualDataCache [i] [j]&& this.isBusy == false) {
					value++;
					if (chartType == ChartType.Line ) {
						this.lineData [lineIndex] [j] = value;

						this.ManualLineChart.UpdateData (lineData);
					} else {
						this.barData [barIndex] [j] = value;
						this.ManualBarChart.UpdateData (barData);
					}

					yield return new WaitForEndOfFrame ();
				}
				j++;
				if (j >= this.MaxDataLength) {
					j = 0;
					i++;
					if (chartType == ChartType.Bar) {
						barIndex++;
					} else if(chartType == ChartType.Line) {
						lineIndex++;
					}
					if (i >= this.manualDataCache.Length) {
						break;
					}
				}
			} else {
				if (this.isBusy == false) {
					yield return new WaitForEndOfFrame ();
				}
			}
		}
		print ("should be stop");
	}

	void ResetData(Chart chart, ref float[][] chartData, int series){
		
		chartData= new float[series][];
		for (int i = 0; i < series; i++) {
			chartData [i] = new float[this.MaxDataLength];
			for (int j = 0; j < this.MaxDataLength; j++) {
				chartData [i] [j] = 0;
			}
		}
		chart.UpdateData (barData);
	}
		

}
	