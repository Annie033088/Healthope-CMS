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

    const coaches = [
        {
            CoachId: 1,
            Account: "coachAlice",
            Email: "alice@example.com",
            Phone: 912345678,
            Name: "Alice",
            PhotoUrl: avatar1,
            Introduction: "熱愛健身，擅長塑形。",
            Specialty: "重訓、有氧、TRX、體態雕塑",
            Certification: "ACE私人教練證照、TRX認證",
            Status: true,
            Type: 1,
            ContractStartTime: "2023-03-01",
            ContractEndTime: "2025-03-01",
            CreateTime: "2025-05-15T10:00:00",
            UpdateTime: "2025-05-15T10:00:00"
        },
        {
            CoachId: 2,
            Account: "coachBob",
            Email: "bob@example.com",
            Phone: 923456781,
            Name: "Bob",
            PhotoUrl: avatar1,
            Introduction: "注重學術背景的教練。",
            Specialty: "運動科學、體能訓練、姿勢調整",
            Certification: "NSCA認證、運動傷害預防課程",
            Status: false,
            Type: 0,
            ContractStartTime: "2022-07-15",
            ContractEndTime: "2024-07-15",
            CreateTime: "2025-05-15T10:00:00",
            UpdateTime: "2025-05-15T10:00:00"
        },
        {
            CoachId: 3,
            Account: "coachCathy",
            Email: "cathy@example.com",
            Phone: 934567892,
            Name: "Cathy",
            PhotoUrl: avatar1,
            Introduction: "專攻女性體態與飲食調整。",
            Specialty: "孕婦運動、飲食控制、塑身",
            Certification: "CPR認證、孕婦運動專業證書",
            Status: true,
            Type: 2,
            ContractStartTime: "2024-01-10",
            ContractEndTime: "2025-12-31",
            CreateTime: "2025-05-15T10:00:00",
            UpdateTime: "2025-05-15T10:00:00"
        },
        {
            CoachId: 4,
            Account: "coachDaniel",
            Email: "daniel@example.com",
            Phone: 945678903,
            Name: "Daniel",
            PhotoUrl: avatar1,
            Introduction: "擁有豐富比賽經驗的選手。",
            Specialty: "CrossFit、比賽備賽、爆發力訓練",
            Certification: "CrossFit L1證照、運動營養學證書",
            Status: true,
            Type: 1,
            ContractStartTime: "2021-11-05",
            ContractEndTime: "2026-11-05",
            CreateTime: "2025-05-15T10:00:00",
            UpdateTime: "2025-05-15T10:00:00"
        },
        {
            CoachId: 5,
            Account: "coachEmma",
            Email: "emma@example.com",
            Phone: 956789014,
            Name: "Emma",
            PhotoUrl: avatar1,
            Introduction: "親切又有效率的教學風格。",
            Specialty: "初學者教學、塑形、營養諮詢",
            Certification: "體適能C級證照、營養諮詢師",
            Status: false,
            Type: 0,
            ContractStartTime: "2023-08-20",
            ContractEndTime: "2025-08-20",
            CreateTime: "2025-05-15T10:00:00",
            UpdateTime: "2025-05-15T10:00:00"
        },
        {
            CoachId: 6,
            Account: "coachChaCha",
            Email: "",
            Phone: 989056744,
            Name: "ChaCha",
            PhotoUrl: avatar1,
            Introduction: "熱愛健身，擅長塑形。",
            Specialty: "重訓、有氧、TRX、體態雕塑",
            Certification: "ACE私人教練證照、TRX認證",
            Status: true,
            Type: 1,
            ContractStartTime: "0001-01-01",
            ContractEndTime: "0001-01-01",
            CreateTime: "2025-05-15T10:00:00",
            UpdateTime: "2025-05-15T10:00:00"
        },
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

    // Schedule
    mock.onPost("/api/GroupClassSchedule/GetShowcaseAndCoach").reply(config => {
        let getShowcaseDto = JSON.parse(config.data);

        let filteredShowcase = courseList.filter(item => {
            const matchCategory = getShowcaseDto.Category === null || item.Category === Number(getShowcaseDto.Category);
            return matchCategory;
        });

        const ShowcaseList = filteredShowcase;

        return [200, { ErrorCode: 1, ApiDataObject: {ShowcaseList, CoachList:coaches} }]
    })
    
    mock.onPost("/api/GroupClassSchedule/AddSchedule").reply(() => {
        return [200, { ErrorCode: 1 }]
    })
}