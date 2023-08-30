-- Data for Ubiquiti hardware
INSERT INTO [dbo].[UniFiPricingTable] ([Id], [MachineName], [RackSize], [DeviceType], [Price], [Producent])
VALUES
(1, 'UniFi Security Gateway', '1U', 'Router with integrated firewall', '769.00', 'Ubiquiti'),
(2, 'UniFi Switch 24', '1U', 'Switch', '1200.00', 'Ubiquiti'),
(3, 'UniFi AP AC Pro', '0U', 'WiFi Hotspot', '500.00', 'Ubiquiti');

-- Data for Juniper hardware
INSERT INTO [dbo].[UniFiPricingTable] ([Id], [MachineName], [RackSize], [DeviceType], [Price], [Producent])
VALUES
(4, 'Juniper SRX340', '1U', 'Router with integrated firewall', '3000.00', 'Juniper'),
(5, 'Juniper EX3300', '1U', 'Switch', '2400.00', 'Juniper'),
(6, 'Juniper WLC880', '1U', 'WiFi Hotspot', '1000.00', 'Juniper');

-- Data for Mikrotik hardware
INSERT INTO [dbo].[UniFiPricingTable] ([Id], [MachineName], [RackSize], [DeviceType], [Price], [Producent])
VALUES
(7, 'RB2011UiAS-RM ', '1U', 'Router with integrated firewall', '600.00', 'Mikrotik'),
(8, 'TL SG3210 8p', '1U', 'Switch', '5440.00', 'TP-Link'),
(9, 'TL SG2218 16p', '1U', 'Switch', '656.00', 'TP-LInk'),
(10, 'TL SG3428 24p', '1U', 'Switch', '892.00', 'TP-Link'),
(11, 'NanoStation 5AC Loco NS-5ACL', '0U', 'WiFi Hotspot', '300.00', 'Ubiquiti'),
(12, 'EAP245', '0U', 'WiFi Hotspot', '538.00', 'TP-Link'),
(13, 'Juniper WLC880', '0U', 'WiFi Hotspot', '1000.00', 'TP-Link');
