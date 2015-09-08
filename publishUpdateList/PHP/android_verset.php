<?php

define('VERSION_FILE', './android_ver.js');

$content = $_POST['content'];

if (empty($content) || $content == '')
	die('版本信息内容不能为空');

if ($content == 'GET') {
	@$fp = fopen(VERSION_FILE, 'r');
	if ($fp) {
		while (!feof($fp)) {
			$output = fgetss($fp,65535);
			echo $output;
		}
		fclose($fp);
		die();
	} else {
		die('['.VERSION_FILE.']file not found.!');
	}
}

$data = json_decode($content);
if ($data == null)
	die('版本信息json文本中含有错误的语法！');
	
$file = fopen(VERSION_FILE, 'w');
if ($file == null)
	die('无法将内容写入'.VERSION_FILE.'，请检查文件权限！');
fwrite($file, $content);
fflush($file);
fclose($file);
echo 'OK';