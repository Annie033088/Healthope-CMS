export const groupClassIcon = [
    { value: '1', text: '❓' },
    { value: '2', text: '🏃‍♂️' },
    { value: '3', text: '❤️' },
    { value: '4', text: '⏱️' },
    { value: '5', text: '💪' },
    { value: '6', text: '🏋️' },
    { value: '7', text: '🏋️‍♂️' },
    { value: '8', text: '🧘‍♀️' },
    { value: '9', text: '🧘‍♂️' },
    { value: '10', text: '💃' },
    { value: '11', text: '🎶' },
    { value: '12', text: '🪩' },
    { value: '13', text: '👟' },
    { value: '14', text: '🚴‍♀️' },
    { value: '15', text: '🚲' },
    { value: '16', text: '💨' },
    { value: '17', text: '⭐' },
    { value: '18', text: '🪜' },
    { value: '19', text: '🎯' },
    { value: '20', text: '🔄' },
    { value: '21', text: '🎲' },
    { value: '22', text: '🧎' },
    { value: '23', text: '🕺' },
]

export function valueToIcon(value) {
    const match = groupClassIcon.find(icon => icon.value === value.toString());
    return match ? match.text : null;
}

export const groupClassCategory = {
    Cardio: 1,
    Force: 2,
    Yoga: 3,
    Dance: 4,
    Flywheel: 5,
    Basic: 6,
    Other: 7
}

export const groupClassCategoryReverse = Object.fromEntries(
    Object.entries(groupClassCategory).map(([k, v]) => [v, k])
);

export const groupClassCategoryAndText = [
    { value: '1', text: '有氧' },
    { value: '2', text: '肌力' },
    { value: '3', text: '瑜伽' },
    { value: '4', text: '舞蹈' },
    { value: '5', text: '飛輪' },
    { value: '6', text: '基礎' },
    { value: '7', text: '其他' },
]

export const groupClassScheduleStatus = [
    { value: '1', text: '未開放' },
    { value: '2', text: '開放' },
    { value: '3', text: '結束' },
    { value: '4', text: '取消' },
]