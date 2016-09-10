<?php 
        $db = mysql_connect("localhost", "root", "") or die('Could not connect: ' . mysql_error()); 
        mysql_select_db("Unity") or die('Could not select database');
 
        // Strings must be escaped to prevent SQL injection attack. 
        $fbid = mysql_real_escape_string($_GET["fbid"], $db); 
        $score = mysql_real_escape_string($_GET["score"], $db); 
		
		 $query = "SELECT * FROM Score where fb_id='".$fbid."'";
    	 $result = mysql_query($query) or die('Query failed: ' . mysql_error());
   		 $num_results = mysql_num_rows($result);
		 
		if($num_results>0){
			// Send variables for the MySQL database class. 
            $query1 = "update Score set score='".$score."' where fb_id='".$fbid."'"; 
            $result1 = mysql_query($query1) or die('Query failed: ' . mysql_error()); 
		}else{
			// Send variables for the MySQL database class. 
            $query2 = "insert into Score values (NULL, '$fbid', '$score');"; 
            $result2 = mysql_query($query2) or die('Query failed: ' . mysql_error()); 
		}
		
            
?>