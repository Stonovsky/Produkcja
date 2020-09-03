# Production

## Description
WPF MVVM Application made for internal use inside the company. The general purpose of this app is to:
1. Get complete information about company group from different ERP systems
2. Control financial requests
3. Get complete information about production nests and manufactured product incl. traceability
4. Get Laboratory analysis of manufactured goods
5. Ease the warehouse movement of manufactured products with external ERPs
6. Summary analyze of condition of company group in given dates

## Stack
1. .Net Framework 4.7.2
2. WPF MVVM + MaterialDesign
3. Ms SQL Server
4. ORM
    - Entity Framework 6 (code first)
    - Dapper
4. IoC (Autofac)
5. nUnit

App consists of different modules listed below and combine information from different environment such as Subiekt GT, Comarch Optima, Ma Access.

## Modules:
### Financial requests
  - Add financial requests and categorize them. Than
  - Accept financial requests by supervisor
  - Analyze financial request by category, i.e. what is the cost of a certain production machine in time

### Customers
- Add and manage customers

### Warehouses
- Get the information about virtual warehouses from different environment

### Production
- Create production request
- Registration of production on production nest in order to minimize data that has to be typed by machine operator
  - labelling in time (using ZPL language and Zebra printers)
  - weight in time (using rs232 protocol on scales)
- Advance analysis of production request
- Adding and managing of laboratory test of a product
- Generates *.epp files for ease of use with Subiekt GT

### Information from external environments
- It grabs data such as: Customers, orders, sales, warehouse, receivables and liabilities from Subiekt GT and Comarch Optima ERP on order to have complete knowledge about company group

### Analysis
- Production requests
- Order request
- Sales
- General finance analysis in given dates (orders, sales, warehouse, receivables and liabilities, production capacity, etc.)
- Production analysis


Couple screens from the app:

Login page:

![Login](https://github.com/Stonovsky/GAT_Produkcja/blob/master/GAT_Produkcja/Images/LoginPage_clear.png)

Main page:

![Main](https://github.com/Stonovsky/GAT_Produkcja/blob/master/GAT_Produkcja/Images/Main_clear.png)

List of manufatured goods in given dates:
![Production](https://github.com/Stonovsky/GAT_Produkcja/blob/master/GAT_Produkcja/Images/EwidencjaProdukcji_clear.png)
# Produkcja
