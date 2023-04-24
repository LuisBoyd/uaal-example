<?php

include "DotEnv.php";

(new DotEnv(__DIR__ . '/.env'))->load();

class MySqlDb{
    protected $host;
    protected $username;
    protected $password;
    protected $db;
    protected $conn;
    function __construct(){
        $this->host = getenv('mysql_host');
        $this->username = getenv('mysql_username');
        $this->password = getenv('mysql_password');
        $this->db = getenv('mysql_database');
        $this->connect();
    }
    private function connect(){
        $this->conn = new mysqli($this->host, $this->username, $this->password, $this->db);
        if ($this->conn->connect_error) {
            die("Connection failed: " . $this->conn->connect_error);
        }
    }

    function getTable($table)
    {
        $sql = "SELECT * FROM ".$table;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        $a = array();
        while($row = $sql->fetch_assoc()){
            array_push($a, $row);
        }
        return $a;
    }

    function getFieldsFromTable($table, ...$fields){
        $result = null;
        foreach ($fields as $field){
            $result = $result.','.$field;
        }
        $result = ltrim($result, $result[0]);
        $sql = "SELECT ".$result." FROM ".$table;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        $a = array();
        while($row = $sql->fetch_assoc()){
            array_push($a, $row);
        }
        return $a;
    }

    function getFieldsFromTableWhere($table,$where, ...$fields){
        $result = null;
        foreach ($fields as $field){
            $result = $result.','.$field;
        }
        $result = ltrim($result, $result[0]);
        $sql = "SELECT ".$result." FROM ".$table." WHERE ".$where;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        return $sql;
    }


    function getData($table, $where){
        //$this->connect();
        $sql = "SELECT * FROM ".$table." WHERE ".$where;
        $sql = $this->conn->query($sql) or die($this->conn->error);;
        $sql = $sql->fetch_assoc();
        return $sql;
    }

    function GetRows($table, $where){
        $sql = "SELECT * FROM ".$table." WHERE ".$where;
        $sql = $this->conn->query($sql) or die($this->conn->error);;
        $values = array();
        while ($row = $sql->fetch_assoc()){
            array_push($values, $row);
        }
        return $values;
    }

    function RawStatement($sqlStatement){
        $sql = $sqlStatement;
        $sql = $this->conn->query($sql) or die($this->conn->error);;
        $sql = $sql->fetch_assoc();
        return $sql;
    }

    function RawStatementRawResult($sqlStatement)
    {
        $sql = $sqlStatement;
        $sql = $this->conn->query($sql) or die($this->conn->error);;
        return $sql;
    }

    function RawStatementArray($sqlStatement)
    {
        $sql = $sqlStatement;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        $a = array();
        while ($row = $sql->fetch_assoc()){
            array_push($a, $row);
        }
        return $a;
    }

    function GetCount($table, $where)
    {
        $sql = "SELECT Count(*) as Quantity FROM ".$table." WHERE ".$where;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        $sql = $sql->fetch_assoc();
        return $sql['Quantity'];
    }

    function DoesRowExist($table, ...$where)
    {
        $this->connect();
        $whereQuery = implode(" AND ", $where);
        $sql = "SELECT COUNT(*) as Quantity FROM ".$table." WHERE ".$whereQuery;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        $sql = $sql->fetch_assoc();
        $result = $sql['Quantity'];
        if($result <= 0){
            return false;
        }
        else{
            return true;
        }
    }

    function updateData($table, $update_value, $where){
        $this->connect();
        $sql = "UPDATE ".$table." SET ".$update_value." WHERE ".$where;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        if($sql == true){
            return true;
        }else{
            return false;
        }
    }
    function createData($table, $columns, $values){
        $this->connect();
        $sql = "INSERT INTO ".$table." ".$columns." VALUES ".$values;
        $sql = $this->conn->query($sql) or die($this->conn->error);
        return $sql;
    }

    function deleteData($table, $filter){
        $this->connect();
        $sql =  "DELETE FROM ".$table." ".$filter;
        $sql = $this->conn->query($sql) or die($this->conn->error);;
        if($sql == true){
            return true;
        }else{
            return false;
        }
    }

}
?>