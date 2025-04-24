import MockAdapter from 'axios-mock-adapter'
import axios from 'axios'

const mock = new MockAdapter(axios)

// 自動載入其他 mock 模組
const context = require.context('./api', false, /\.js$/)
context.keys().forEach((key) => {
  const setup = context(key).default
  if (typeof setup === 'function') {
    // 把 mock 傳進去使用
    setup(mock) 
  }
})

export default mock