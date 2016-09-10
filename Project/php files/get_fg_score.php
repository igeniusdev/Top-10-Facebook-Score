<?php
    // Send variables for the MySQL database class.
    $database = mysql_connect("localhost", "root", "") or die('Could not connect: ' . mysql_error());
    mysql_select_db("Unity") or die('Could not select database');
 
    $query = "SELECT * FROM Score ORDER by score DESC LIMIT 10";
    $result = mysql_query($query) or die('Query failed: ' . mysql_error());
 
    $num_results = mysql_num_rows($result);
    for($i = 0; $i < $num_results; $i++)
    {
         $row = mysql_fetch_array($result);
		 if($i==0){
			  echo $row['fb_id'] . "@" . $row['score'];
		 }else{
        	 echo ",".$row['fb_id'] . "@" . $row['score'];
		 }
		 
    }
	if($num_results==0){
		echo "No Score Found";
	}

?>