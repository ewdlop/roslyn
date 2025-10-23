// 標籤頁切換功能
document.addEventListener('DOMContentLoaded', function() {
    // 標籤頁按鈕
    const tabButtons = document.querySelectorAll('.tab-btn');
    const tabContents = document.querySelectorAll('.tab-content');

    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            const targetTab = button.getAttribute('data-tab');
            
            // 移除所有 active 類
            tabButtons.forEach(btn => btn.classList.remove('active'));
            tabContents.forEach(content => content.classList.remove('active'));
            
            // 添加 active 類到點擊的按鈕和對應的內容
            button.classList.add('active');
            document.getElementById(targetTab).classList.add('active');
        });
    });

    // 平滑滾動
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // 滾動動畫
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, observerOptions);

    // 觀察所有內容卡片
    document.querySelectorAll('.content-card, .feature-card, .community-card, .workflow-section').forEach(el => {
        el.classList.add('fade-in');
        observer.observe(el);
    });

    // 導航欄滾動效果
    let lastScrollTop = 0;
    const navbar = document.querySelector('.navbar');
    
    window.addEventListener('scroll', () => {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        
        if (scrollTop > lastScrollTop && scrollTop > 100) {
            // 向下滾動
            navbar.style.transform = 'translateY(-100%)';
        } else {
            // 向上滾動
            navbar.style.transform = 'translateY(0)';
        }
        
        lastScrollTop = scrollTop;
    });

    // 添加導航欄背景模糊效果
    window.addEventListener('scroll', () => {
        if (window.scrollY > 50) {
            navbar.style.backdropFilter = 'blur(20px)';
            navbar.style.background = 'rgba(13, 17, 23, 0.8)';
        } else {
            navbar.style.backdropFilter = 'blur(10px)';
            navbar.style.background = 'rgba(13, 17, 23, 0.95)';
        }
    });

    // 為程式碼區塊添加複製按鈕
    document.querySelectorAll('.code-example, .code-block').forEach(block => {
        const copyButton = document.createElement('button');
        copyButton.className = 'copy-button';
        copyButton.innerHTML = '📋 複製';
        copyButton.style.cssText = `
            position: absolute;
            top: 10px;
            right: 10px;
            background: rgba(81, 43, 212, 0.8);
            color: white;
            border: none;
            padding: 0.5rem 1rem;
            border-radius: 6px;
            cursor: pointer;
            font-size: 0.9rem;
            transition: all 0.3s;
            opacity: 0;
            z-index: 10;
        `;
        
        block.style.position = 'relative';
        block.appendChild(copyButton);

        block.addEventListener('mouseenter', () => {
            copyButton.style.opacity = '1';
        });

        block.addEventListener('mouseleave', () => {
            copyButton.style.opacity = '0';
        });

        copyButton.addEventListener('click', () => {
            const code = block.querySelector('code').textContent;
            navigator.clipboard.writeText(code).then(() => {
                copyButton.innerHTML = '✓ 已複製';
                copyButton.style.background = 'rgba(63, 185, 80, 0.8)';
                
                setTimeout(() => {
                    copyButton.innerHTML = '📋 複製';
                    copyButton.style.background = 'rgba(81, 43, 212, 0.8)';
                }, 2000);
            });
        });
    });

    // 動態數字計數效果
    function animateValue(element, start, end, duration) {
        let startTimestamp = null;
        const step = (timestamp) => {
            if (!startTimestamp) startTimestamp = timestamp;
            const progress = Math.min((timestamp - startTimestamp) / duration, 1);
            const value = Math.floor(progress * (end - start) + start);
            element.textContent = value.toLocaleString();
            if (progress < 1) {
                window.requestAnimationFrame(step);
            }
        };
        window.requestAnimationFrame(step);
    }

    // 當統計數字進入視窗時開始動畫
    const statsObserver = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const statValue = entry.target;
                const text = statValue.textContent.replace(/[^0-9]/g, '');
                if (text) {
                    const endValue = parseInt(text);
                    animateValue(statValue, 0, endValue, 2000);
                }
                statsObserver.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });

    document.querySelectorAll('.stat-value, .stat-number').forEach(stat => {
        statsObserver.observe(stat);
    });

    // 為管線階段添加點擊展開效果
    document.querySelectorAll('.pipeline-stage').forEach(stage => {
        stage.addEventListener('click', () => {
            stage.classList.toggle('expanded');
        });
    });

    // 為架構層添加懸停效果
    document.querySelectorAll('.layer').forEach((layer, index) => {
        layer.addEventListener('mouseenter', () => {
            layer.style.transform = `translateX(${(index + 1) * 10}px)`;
        });
        
        layer.addEventListener('mouseleave', () => {
            layer.style.transform = 'translateX(0)';
        });
    });

    // 添加鍵盤導航支援
    document.addEventListener('keydown', (e) => {
        if (e.key === 'ArrowLeft' || e.key === 'ArrowRight') {
            const activeTab = document.querySelector('.tab-btn.active');
            if (activeTab) {
                const tabs = Array.from(document.querySelectorAll('.tab-btn'));
                const currentIndex = tabs.indexOf(activeTab);
                let newIndex;
                
                if (e.key === 'ArrowLeft') {
                    newIndex = currentIndex > 0 ? currentIndex - 1 : tabs.length - 1;
                } else {
                    newIndex = currentIndex < tabs.length - 1 ? currentIndex + 1 : 0;
                }
                
                tabs[newIndex].click();
            }
        }
    });

    // 添加深色/淺色主題切換（預留功能）
    const themeToggle = document.createElement('button');
    themeToggle.innerHTML = '🌙';
    themeToggle.className = 'theme-toggle';
    themeToggle.style.cssText = `
        position: fixed;
        bottom: 2rem;
        right: 2rem;
        width: 50px;
        height: 50px;
        border-radius: 50%;
        background: var(--gradient-1);
        color: white;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.3);
        transition: all 0.3s;
        z-index: 1000;
    `;
    
    document.body.appendChild(themeToggle);

    themeToggle.addEventListener('mouseenter', () => {
        themeToggle.style.transform = 'scale(1.1)';
    });

    themeToggle.addEventListener('mouseleave', () => {
        themeToggle.style.transform = 'scale(1)';
    });

    // 添加回到頂部按鈕
    const backToTop = document.createElement('button');
    backToTop.innerHTML = '↑';
    backToTop.className = 'back-to-top';
    backToTop.style.cssText = `
        position: fixed;
        bottom: 2rem;
        right: 5rem;
        width: 50px;
        height: 50px;
        border-radius: 50%;
        background: var(--gradient-1);
        color: white;
        border: none;
        font-size: 1.5rem;
        cursor: pointer;
        box-shadow: 0 5px 20px rgba(0, 0, 0, 0.3);
        transition: all 0.3s;
        opacity: 0;
        visibility: hidden;
        z-index: 1000;
    `;
    
    document.body.appendChild(backToTop);

    window.addEventListener('scroll', () => {
        if (window.scrollY > 500) {
            backToTop.style.opacity = '1';
            backToTop.style.visibility = 'visible';
        } else {
            backToTop.style.opacity = '0';
            backToTop.style.visibility = 'hidden';
        }
    });

    backToTop.addEventListener('click', () => {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    backToTop.addEventListener('mouseenter', () => {
        backToTop.style.transform = 'scale(1.1) translateY(-5px)';
    });

    backToTop.addEventListener('mouseleave', () => {
        backToTop.style.transform = 'scale(1) translateY(0)';
    });

    // 為導航連結添加活動狀態
    const sections = document.querySelectorAll('section[id]');
    const navLinks = document.querySelectorAll('.nav-links a');

    window.addEventListener('scroll', () => {
        let current = '';
        sections.forEach(section => {
            const sectionTop = section.offsetTop;
            const sectionHeight = section.clientHeight;
            if (window.scrollY >= sectionTop - 200) {
                current = section.getAttribute('id');
            }
        });

        navLinks.forEach(link => {
            link.classList.remove('active');
            if (link.getAttribute('href').slice(1) === current) {
                link.classList.add('active');
            }
        });
    });

    // 添加視差效果到 hero 背景
    window.addEventListener('scroll', () => {
        const heroBackground = document.querySelector('.hero-background');
        if (heroBackground) {
            const scrolled = window.scrollY;
            heroBackground.style.transform = `translateY(${scrolled * 0.5}px)`;
        }
    });

    console.log('🎉 Roslyn 網站已載入完成！');
    console.log('📊 專案統計：20,000+ 檔案，100+ 貢獻者');
    console.log('🔗 GitHub: https://github.com/dotnet/roslyn');
});

