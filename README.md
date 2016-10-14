# VRChart
Simple chart demo
##How to use
* Drag a DataVisualization prefab to you scene
* Create a script file and attach it to your camera or other object
* Edit the new script file as follow:
  * Obtain a DynamicData script object from DataVisualization
  * Use Series class to create some data series
  * Call DynamicData's method AddSeries() to add those series
  * Call DynamicData's method Render() to render those series
  * Call DynamicData's method Next() to response User's action
  * You can use DynamicData's method ShowAll() to show all series immediately
  * Please see the simple.cs file


