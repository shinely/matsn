git操作
#初始化一个本地仓库 （将你现在操作的路径设置为本地仓库，成功后会多出一个.git的目录）
git init 
#从现有的仓库克隆
git clone git@github.com:shinely/matsn.git
#克隆时可以指定自己想要的名字
git clone git@github.com:shinely/matsn.git test
#查看提交历史  -p显示每次提交的差异 -2最近2次提交
git log -p -2

#设备用户名和邮箱
git config --global user.name 'matsn'
git config --global user.email ‘meiqingqing8@qq.com'
#查看已经设置的用户名和邮箱
git config --global user.name
git config --global user.email
#将文件添加到本地仓库
touch hello    //创建文件
git add hello
#取消暂存 （添加）
git reset HEAD hello
#取消对文件的修改
git checkout -- hello

#从本地仓库中删除文件
git rm hello
#提交到本地仓库并备注 (此时文件仍然在本地)
git commit -m "add file hello" 
#自动更新变化的文件 -a auto
git commit -a  
#修改最后一次提交  下面三行命令仅执行一次提交，第二次提交命令修正了第一次提交内容
git commit -m "initial commit"
git add forgotten_file
git commit --amend
 
#查看当前配置有哪些远程仓库 -v(--verbose)显示对应的克隆地址
git remote -v 
#增加一个远程服务器的别名
git remote add matsn git@github.com:shinely/matsn.git
#删除远程版本库的别名
git remote rm matsn
#将本地仓库master提交到Github的shinely/matsn版本库中 更新本地文件到github服务器
git push -u git@github.com:shinely/matsn master
git push -u matsn master
#从github服务器仓库下载文件  获取到本地不合并
git fetch git@github.com:shinely/matsn.git
#获取并合并内容到本地
git pull git@github.com:shinely/matsn.git

#配置github ssh key
#在本地创建ssh key,回车后根据提示输入要保存key的文件名，然后密码一直按回车即可，
#会生成的key和key.pub两个文件，把公钥key.pub里的密钥复制；
#在github页面，进入Account Settings，左边选择SSH and GPG Keys,选择SSH keys，new SSH key，粘贴即可.
ssh-keygen -t -rsa -C "35815768@qq.com"
#添加密钥到ssh-agent
#确保ssh-agent密钥管理器是可用的
eval "$(ssh-agent -s)"
#将生成的私钥添加到密钥管理器
ssh-add key
#验证ssh是否设置成功
ssh -T git@github.com

#显示当前分支
git branch
#创建分支
git branch new-feature
#切换到新分支
git checkout new-feature
#提交新分支
git commit -a -m "new feature"
#将分支提交到服务器  内容提交，并未和主干发生合并
git push git@github.com:shinely/matsn new feature

#new feature分支成熟，合并进master
#切换到主干
git checkout master
#把分支合并到主干
git merge new-feature
#显示当前分支是master
git branch
#删除分支
git branch -d new-feature
#提交
git push




