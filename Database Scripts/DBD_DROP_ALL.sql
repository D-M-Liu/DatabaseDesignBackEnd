-- É¾³ý±í C##CAR.SWITCH_REQUEST_EMPLOYEE
DROP TABLE C##CAR.SWITCH_REQUEST_EMPLOYEE CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.MAINTENANCE_ITEM_EMPLOYEE
DROP TABLE C##CAR.MAINTENANCE_ITEM_EMPLOYEE CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.MAINTENANCE_ITEM
DROP TABLE C##CAR.MAINTENANCE_ITEM CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.EMPLOYEE_SWITCH_STATION
DROP TABLE C##CAR.EMPLOYEE_SWITCH_STATION CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.VEHICLE
DROP TABLE C##CAR.VEHICLE CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.SWITCH_REQUESTS
DROP TABLE C##CAR.SWITCH_REQUESTS CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.KPI
DROP TABLE C##CAR.KPI CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.SWITCH_LOG
DROP TABLE C##CAR.SWITCH_LOG CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.BATTERY_SWITCH_STATION
DROP TABLE C##CAR.BATTERY_SWITCH_STATION CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.NEWS
DROP TABLE C##CAR.NEWS CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.EMPLOYEE
DROP TABLE C##CAR.EMPLOYEE CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.VEHICLE_OWNER
DROP TABLE C##CAR.VEHICLE_OWNER CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.VEHICLE_PARAM
DROP TABLE C##CAR.VEHICLE_PARAM CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.BATTERY
DROP TABLE C##CAR.BATTERY CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.BATTERY_TYPE
DROP TABLE C##CAR.BATTERY_TYPE CASCADE CONSTRAINTS;

-- É¾³ý±í C##CAR.SWITCH_STATION
DROP TABLE C##CAR.SWITCH_STATION CASCADE CONSTRAINTS;

-- É¾³ýËùÓÐ´¥·¢Æ÷
DROP TRIGGER c##car.trg_cascade_update_battery;
DROP TRIGGER c##car.trg_bat_bat_replacing_sites_battery;
DROP TRIGGER c##car.trg_bat_bat_replacing_sites_site;
DROP TRIGGER c##car.trg_kpi_employee;
DROP TRIGGER c##car.trg_vehicle_owner;
DROP TRIGGER c##car.trg_vehicle_vehicle_param;
DROP TRIGGER c##car.trg_vehicle_battery;
DROP TRIGGER c##car.trg_maintenance_item_vehicle;
DROP TRIGGER c##car.trg_employee_SWITCH_station_employee;
DROP TRIGGER c##car.trg_employee_SWITCH_station_site;
DROP TRIGGER c##car.trg_maintenance_item_employee_employee;
DROP TRIGGER c##car.trg_maintenance_item_employee_maintenance_item;
DROP TRIGGER c##car.trg_switch_request_employee_employee;
DROP TRIGGER c##car.trg_switch_request_vehicle;
DROP TRIGGER c##car.trg_switch_request_acceptance;
DROP TRIGGER c##car.trg_switch_log_vehicle;
DROP TRIGGER c##car.trg_switch_log_employee;
DROP TRIGGER c##car.trg_switch_log_battery_in;
DROP TRIGGER c##car.trg_switch_log_battery_out;
DROP TRIGGER c##car.trg_switch_log_switch_request_lon;
DROP TRIGGER c##car.trg_switch_log_switch_request_lat;
