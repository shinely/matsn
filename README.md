git操作
#初始化一个本地仓库 （将你现在操作的路径设置为本地仓库，成功后会多出一个.git的目录）
git init 
#设备用户名和邮箱
git config --global user.name 'matsn'
git config --global user.email ‘meiqingqing8@qq.com'
#查看已经设置的用户名和邮箱
git config --global user.name
git config --global user.email
#将文件添加到本地仓库
touch hello    //创建文件
git add hello
#从本地仓库中删除文件
git rm hello
#提交到本地仓库并备注 (此时文件仍然在本地)
git commit -m "add file hello" 
#自动更新变化的文件 -a auto
git commit -a  
#增加一个远程服务器的别名
git remote add matsn git@github.com:shinely/matsn
#删除远程版本库的别名
git remote rm matsn
#将本地仓库master提交到Github的shinely/matsn版本库中 更新本地文件到github服务器
git push -u git@github.com:shinely/matsn master
git push -u matsn master











matsn
