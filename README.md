# SensorTag Azure
This project is designed to demonstrate how from a device via Microsoft Azure can display information in PowerBI. (This project got much inspiration from the following project [SensorTagToEventHub]( https://github.com/Azure/azure-stream-analytics/tree/master/Samples/SensorDataAnalytics/SensorTagToEventHub).)

## Overview
A *Universal Windows App* connects to the *SensorTag* with *Bluetooth*. This app reads the sensor each second and sends the data to *Azure EventHub*. An *Azure Stream Analytics* job analyse the data and send it to *PowerBI*.

## What you need
* Windows 10 computer
* TI SensorTag
* Windows Azure Account using Org Id (Power BI works with Org ID only. Org ID is your work or business email address e.g. xyz@mycompany.com. Personal emails like xyz@hotmail.com are not org ids. [You can learn more about org id here](https://www.arin.net/resources/request/org.html) )

## Getting started

### 1. Pair the TI SensorTag in the *Manage Bluetooth devices* settings
> To make sure that the demonstration work, it is a good practice to remove other SensorTags that are already paired in the system.

### 2. Download the source code or the compiled executable.

### 3. Create EventHub and Stream Analytics services in Azure
1\. Open [http://manage.windowsazure.com](http://manage.windowsazure.com)

2\. Navigate to the *Service Bus pane*.

![Service Bus](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/service-bus.png)

3\. Click on the plus sign to create a new *Service Bus Namespace* and enter the information seen in the screenshot below.

![New Service Bus Namespace](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/service-bus-create-namespace.png)

4\. Click on the new Service Bus namespace.

5\. Open the tab *EVENT HUBS* and *Create a New Event Hub* in your Service Bus namespace.

![Create new Event Hub](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/service-bus-create-event-hub-start.png)

6\. Enter the following information.

![Create new Event Hub](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-create.png)

7\. Click on the new Event hub after it is created.

![New Event Hub Ready](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-created.png)

8\. Click on the tab *CONFIGURE* and enter the information shown in the screen below in the section *shared access policy*. Remember
to also save it by clicking SAVE in teh bottom of the screen.

![New Shared Access Policy](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-shared-access-policy.png)

When it is saved a new section will appear, *shared access key generator*. The *Policy Name* and the *Primary Key* will we
use later in the application that sends the information to Azure. (This is the credentials to be able to send to this event hub.)

![Shared Access Key Generator](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-shared-access-key-generator.png)

#### Ready to test sendning the information to Azure
We are now ready to start sending information from the SensorTag into Azure.

1\. Start SensorTagReader on you computer and enter the information to connect to the Event Hub.

![SensorTagReader](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/sensortagreader-settings.png)

2\. Click on Start and make sure that the SensorTag is connected.

If everything is ok you will see events data appering in the dashboard. (This could take up to ten minutes before you
see any data in the dashboard.)

![Event Hub Diagnostics](https://github.com/buzzfrog/SensorTag-Azure/blob/master/images/event-hub-diagnostics.png)

### 4. Create a PowerBI report


