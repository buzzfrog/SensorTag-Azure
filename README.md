# SensorTag Azure #
This project is designed to demonstrate how from a device via Microsoft Azure can display information in PowerBI. (This project got much inspiration from the following project [SensorTagToEventHub]( https://github.com/Azure/azure-stream-analytics/tree/master/Samples/SensorDataAnalytics/SensorTagToEventHub).)

## Overview ##
A *Universal Windows App* connects to the *SensorTag* with *Bluetooth*. This app reads the sensor each second and sends the data to *Azure EventHub*. An *Azure Stream Analytics* job analyse the data and send it to *PowerBI*.

## What you need ##
* Windows 10 computer
* TI SensorTag
* Windows Azure Account using *Organization account* (Power BI works with Organization account only. Organization account is often your work or business email address e.g. xyz@mycompany.com. 
Personal emails like xyz@hotmail.com are not organizations accounts).

## Getting started ##

### 1. Pair the TI SensorTag in the *Manage Bluetooth devices* settings ###
> To make sure that the demonstration work, it is a good practice to remove other SensorTags that are already paired in the system.

### 2. Download the source code or the compiled executable. ####

### 3. Create EventHub in Azure ####
1\. Open [http://manage.windowsazure.com](http://manage.windowsazure.com)

2\. Navigate to the *Service Bus pane*.

![Service Bus](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/service-bus.png)

3\. Click on the plus sign to create a new *Service Bus Namespace* and enter the information seen in the screenshot below.

![New Service Bus Namespace](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/service-bus-create-namespace.png)

4\. Click on the new Service Bus namespace.

5\. Open the tab *EVENT HUBS* and click on *Create a New Event Hub* in your Service Bus namespace.

![Create new Event Hub](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/service-bus-create-event-hub-start.png)

6\. Enter the following information and click on *Create a new Event Hub*.

![Create new Event Hub](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-create.png)

7\. Click on the new Event hub after it is created.

![New Event Hub Ready](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-created.png)

8\. Click on the tab *CONFIGURE* and enter the information shown in the screen below in the section *shared access policy*. Remember
to also save it by clicking SAVE in the bottom of the screen.

![New Shared Access Policy](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-shared-access-policy.png)

When it is saved a new section will appear, *shared access key generator*. The *Policy Name* and the *Primary Key* will we
use later in the application that sends the information to the EventHub.

![Shared Access Key Generator](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-shared-access-key-generator.png)

#### Ready to test sendning the information to Azure ###
We are now ready to start sending information from the SensorTag into Azure.

1\. Start SensorTagReader on you computer and enter the information to connect to the Event Hub.

![SensorTagReader](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/sensortagreader-settings.png)

2\. Click on Start and make sure that the SensorTag is connected.

If everything is ok you will see events data appering in the dashboard. (This could take up to ten minutes before you
see any data in the dashboard.)

![Event Hub Diagnostics](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-diagnostics.png)

### 4. Create Stream Analytics Jobs in Azure ###
1\. Navigate to *Stream Analytics pane*.

![New Stream Analytics](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-new.png)

2\. Create a new Stream Analytics Job.

![Create New Stream Analytics Job](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-create-new.png)

![Stream Analytics Job](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job.png)

3\. Click on the newly created job to open the details page for that job.

![Stream Analytics Job Detail](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-detail.png)

4\. Click on *INPUTS* in the tab menu to see a list of inputs for this job.

![Stream Analytics Input](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-input.png)

5\. Click on *Add an input*

![Stream Analytics Input Create 1](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-input-create-1.png)

![Stream Analytics Input Create 2](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-input-create-2.png)

![Stream Analytics Input Create 3](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-input-create-3.png)

![Stream Analytics Input Create 4](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-input-create-4.png)

6\. Create a new output by going to the *OUTPUTS* tab

#### VERY IMPORTANT INFORMATION ####
You need to connect to PowerBI with an organization account, as I said above. You can't use a Microsoft Live account for this. So, if you
logged in with a Micrsoft Live account you need to create an Azure Active Directory tenant and create one user in that tenant. Use
that new user when you authorize the connection that we will do in the instructions below.

7\. Click on *Add an output*

![Stream Analytics Output Create](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-output-create-1.png)

8\. Authorize the connection to PowerBI

It is here you need your organizational account.

![Stream Analytics Output Authorize](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-output-create-authorize.png)

9\. PowerBI settings

![Stream Analytics Output Create 3](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-output-create-3.png)

10\. Output created

![Stream Analytics Output Create 3](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-output-created.png)

11\. Time to create the query that analyze the stream. Open the tab *QUERY*.

![Stream Analytics Query](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/stream-analytics-job-query.png)

```
SELECT 
        max(humidity) as humidity,
        max(temperature) as temperature,
        time,
        -20 as minTemperature,
        60 as maxTemperature,
        23 as targetTemperature,
        100 as maxHumidity,
        70 as targetHumidity
FROM Input 
WHERE sensorName = 'sensor001'
Group by TUMBLINGWINDOW(ss,1), time, sensorName
```

Remember to save your query.

12\. Click on *START* in the dark menu in the bottom of the screen to start the job.




### 5. Create a PowerBI report ###

1\. Go to http://www.powerbi.com and login with the same account as you used to authenticate the connection to PowerBI when creating the
output from the Stream Analytics job.

2\. Click on the heading *Power BI* to open your workspace.

![PowerBI Main Page](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/powerbi-main.png)

3\. Click on the hamburger icon up in the left corner to expand the navigation pane.

![PowerBI Workspace](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/powerbi-open-ws.png)

4\. If all previous steps has worked, you will now see that a new dataset called *sensortag* has appeared.

![PowerBI Dataset](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/powerbi-dataset.png)

5\. Click on the dataset *sensortag* to start creating a visualization. 






