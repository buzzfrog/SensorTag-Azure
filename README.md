# SensorTag Azure
This project is designed to demonstrate how from a device via Microsoft Azure can display information in PowerBI.

## Overview
A *Universal Windows App* connects to the *SensorTag* with *Bluetooth*. This app reads the sensor each second and sends the data to *Azure EventHub*. An *Azure Stream Analytics* job analyse the data and send it to *PowerBI*.

## What you need
1. Windows 10 computer
2. TI SensorTag
3. Azure account

## Getting started

### Pair the TI SensorTag in the *Manage Bluetooth devices* settings

