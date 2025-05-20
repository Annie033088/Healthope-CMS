export default function (mock) {
     mock.onPost("/api/GroupClass/AddShowcase").reply((config) => {
        // 可用這方式查看傳輸的資料
        for (let [key, value] of config.data.entries()) {
            console.log(key, value);
        }
        return [200, { ErrorCode: 1 }]
    })
}