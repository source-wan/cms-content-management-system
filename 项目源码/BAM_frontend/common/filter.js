//注册时间createdAt格式化

const filter = {
    formatDate: function(value,createdAt) {
        var dt = new Date(value);
        if(createdAt == 'yyyy-M-d') {// yyyy-M-d
            let year = dt.getFullYear();
            let month = dt.getMonth() + 1;
            let date = dt.getDate();
            return `${year}-${month}-${date}`;
        } else if(createdAt == 'yyyy-M-d H:m:s'){// yyyy-M-d H:m:s
            let year = dt.getFullYear();
            let month = dt.getMonth() + 1;
            let date = dt.getDate();
            let hour = dt.getHours();
            let minute = dt.getMinutes();
            let second = dt.getSeconds();
            return `${year}-${month}-${date} ${hour}:${minute}:${second}`;
        } else if(createdAt == 'yyyy-MM-dd') {// yyyy-MM-dd
            let year = dt.getFullYear();
            let month = (dt.getMonth() + 1).toString().padStart(2,'0');
            let date = dt.getDate().toString().padStart(2,'0');
            return `${year}-${month}-${date}`;
        } else {// yyyy-MM-dd HH:mm:ss
            let year = dt.getFullYear();
            let month = (dt.getMonth() + 1).toString().padStart(2,'0');
            let date = dt.getDate().toString().padStart(2,'0');
            let hour = dt.getHours().toString().padStart(2,'0');
            let minute = dt.getMinutes().toString().padStart(2,'0');
            let second = dt.getSeconds().toString().padStart(2,'0');
            return `${year}-${month}-${date} ${hour}:${minute}:${second}`;
        }
    }
}
export default filter;