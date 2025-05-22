import avatar1 from '@/assets/mockImage/avatar1.png'
export default function (mock) {
    const courseList = [
        {
            GroupClassShowcaseId: 1,
            Name: "正位瑜伽",
            Summary: "YO-GA",
            DetailContent: "⭐Zumba打破了基礎健身的局限性，大膽從音樂風格下手，吸取了健身操和拉丁舞蹈的精華元素，健身者容易盡情投入而不覺得疲倦。舞步自由，可根據自己的特點、對拉丁舞的理解和對音樂的感受詮釋自己的步伐，令身體和心靈上都無束縛，盡情，盡性",
            Category: 3,
            Icon: 8,
            Sort: 1,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 2,
            Name: "有氧燃脂",
            Summary: "",
            DetailContent: "",
            Category: 1,
            Icon: 15,
            Sort: 2,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 3,
            Name: "重量訓練",
            Summary: "",
            DetailContent: "",
            Category: 2,
            Icon: 4,
            Sort: 3,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 4,
            Name: "舞力全開",
            Summary: "",
            DetailContent: "",
            Category: 4,
            Icon: 11,
            Sort: 4,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 5,
            Name: "飛輪挑戰",
            Summary: "",
            DetailContent: "",
            Category: 5,
            Icon: 19,
            Sort: 5,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 6,
            Name: "核心訓練",
            Summary: "",
            DetailContent: "",
            Category: 6,
            Icon: 7,
            Sort: 6,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 7,
            Name: "舒緩伸展",
            Summary: "",
            DetailContent: "",
            Category: 3,
            Icon: 22,
            Sort: 7,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 8,
            Name: "燃脂飛輪",
            Summary: "",
            DetailContent: "",
            Category: 5,
            Icon: 10,
            Sort: 8,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 9,
            Name: "戰繩訓練",
            Summary: "",
            DetailContent: "",
            Category: 2,
            Icon: 6,
            Sort: 9,
            ImageUrl: avatar1
        },
        {
            GroupClassShowcaseId: 10,
            Name: "多元體適能",
            Summary: "",
            DetailContent: "",
            Category: 7,
            Icon: 3,
            Sort: 10,
            ImageUrl: avatar1
        }
    ]

    mock.onPost("/api/GroupClassShowcase/AddShowcase").reply(() => {
        // 可用這方式查看傳輸的資料
        // for (let [key, value] of config.data.entries()) {
        //     console.log(key, value);
        // }
        return [200, { ErrorCode: 1 }]
    })

    mock.onPost("/api/GroupClassShowcase/GetShowcase").reply(config => {
        let {
            Category,
            SortOrder,
            SortOption,
            RecordPerPage,
            SearchName,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = courseList.filter(item => {
            const matchCategory = Category === null || item.Category === Number(Category);
            const matchSearch = !SearchName || item.Name.includes(SearchName);
            return matchCategory && matchSearch;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "sort") {
            field = "Sort";
        } else if (SortOption === "name") {
            field = "Name";
        }
        else {
            field = "GroupClassShowcaseId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            ShowcaseList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })

    mock.onPost("/api/GroupClassShowcase/GetShowcaseDetail").reply((config) => {
        let groupClassShowcaseIdDto = JSON.parse(config.data);
        let groupClassShowcaseTarget = courseList.find(course =>
            course.GroupClassShowcaseId === Number(groupClassShowcaseIdDto.GroupClassShowcaseId));

        if (groupClassShowcaseTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: groupClassShowcaseTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })

    mock.onPost("/api/GroupClassShowcase/GetShowcaseEditDataById").reply(config => {
        let getShowcaseByIdDto = JSON.parse(config.data);
        let showcaseTarget = courseList.find(course => course.GroupClassShowcaseId === Number(getShowcaseByIdDto.GroupClassShowcaseId));

        if (showcaseTarget) {
            return [200, { ErrorCode: 1, ApiDataObject: showcaseTarget }]
        } else {
            return [200, { ErrorCode: 13 }]
        }
    })
    
    mock.onPost("/api/GroupClassShowcase/EditShowcase").reply((config) => {
        //可用這方式查看傳輸的資料
        for (let [key, value] of config.data.entries()) {
            console.log(key, value);
        }
        return [200, { ErrorCode: 1 }]
    })
    
    mock.onPost("/api/GroupClassShowcase/DeleteShowcase").reply(config => {
        let showcaseIdDto = JSON.parse(config.data);
        const index = courseList.findIndex(course => course.GroupClassShowcaseId === Number(showcaseIdDto.GroupClassShowcaseId));

        if (index !== -1) {
          courseList.splice(index, 1);
          return [200, { ErrorCode: 1 }]
        }

        return [200, { ErrorCode: 12 }]
    })
}