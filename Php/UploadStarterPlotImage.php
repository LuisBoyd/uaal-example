<?php
require_once('db_mysql.php');
$mysql = new MySqlDb();
if (isset($_POST['submit'])) {
    if (getimagesize($_FILES['imagefile']['tmp_name']) == false) {
        echo "<br />Please Select An Image.";
    } else {

//declare variables
        $image = $_FILES['imagefile']['tmp_name'];
        $name = $_FILES['imagefile']['name'];
        $MarinaID = $_POST['marinaid'];
        $image = base64_encode(file_get_contents(addslashes($image)));
        $created = $mysql->updateData("DefaultMarinaPlots", "default_plot_data = '$image'",
            "pointOfInterestId = '$MarinaID'");
        if ($created) {
            echo "<br />Image uploaded successfully.";
        } else {
            echo "<br />Image Failed to upload.";
        }

    }

} else {
# code...
}

//Retrieve image from database and display it on html webpage
function displayImageFromDatabase(){
//use global keyword to declare conn inside a function
    $mysql = new MySqlDb();
    $ImageInfo = $mysql->getFieldsFromTable("DefaultMarinaPlots", "default_plot_data");
    foreach ($ImageInfo as $image){
        echo '<img height="250px" width="250px" src=data:image;base64,'.$image['default_plot_data'].'/>';
    }



}
//calling the function to display image
displayImageFromDatabase();
?>
<!DOCTYPE html>
<html>
<head>
    <title>Upload Starter Plot Image</title>
</head>
<body>
<form action="" method="post" enctype="multipart/form-data">
    <input type="number" name="marinaid">
    <br />
    <input type="file" name="imagefile">
    <br />
    <input type="submit" name="submit" value="Upload">

</form>
</body>
</html>
