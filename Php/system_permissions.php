<?php



function checkPermissions($user_id, $permission_id) {

    require_once('db_mysql.php');
    try {
        $mysql = new MySqlDb();
        $stmt = $mysql->RawStatement("select

                     count(*) as total_permissions

                     from system_permission_to_roles

                     left join system_users_to_roles

                     on system_permission_to_roles.role_id = system_users_to_roles.role_id

                     where system_users_to_roles.user_id = '$user_id'

                     and permission_id = '$permission_id'

                    ");
        if ($stmt['total_permissions'] > 0) {
            return true;
        } else {
           return false;
        }
        return false;
    } catch (Exception $e) {

        echo $e->getMessage();

    }
}