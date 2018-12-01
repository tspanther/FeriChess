# Setting the database
### installing mysql
open the terminal and paste
```bash
    sudo apt-get update
    sudo apt-get install mysql-server
```
### deploying the database

open the terminal and paste
```
    sudo mysql - h localhost -u root < PATH_TO_THE_SCRIPT/skripta.sql
```
## Or
### connect to my databse using:
```
host:		creativepowercell.asuscomm.com
username:	Uporabnik
password:	FeriChess
```
Plese note that this account has only the permision to use the `SELECT`, `INSERT` and `UPDATE` commands. You can also access phpmyadmin on the same host.

## ER model

![Model](/ERmodel.png)